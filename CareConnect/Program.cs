using CareConnect.Data;
using CareConnect.Interfaces;
using CareConnect.Models;
using CareConnect.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Globalization;
using System.Net.Http;
using System.Security.Authentication;

namespace CareConnect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var httpClientHandler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12
            };

            var builder = WebApplication.CreateBuilder(args);

            var _logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.File($"{builder.Environment.WebRootPath}\\Logs\\Log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            builder.Logging.AddSerilog(_logger);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");            

            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<RecaptchaSettings>(builder.Configuration.GetSection("GoogleRecaptchaV3"));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddTransient<IAuditTrailService, AuditTrailService>();
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddSingleton<UniqueCode>();
            builder.Services.AddSingleton<CustomIDataProtection>();
            builder.Services.AddSingleton<RecaptchaService>();
            builder.Services.AddHttpClient();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = ".CareConnect";
                options.Cookie.Path = "/";
                //options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                //options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
            });

            builder.Services.AddMvc();

            var app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    var dbInitializerLogger = services.GetRequiredService<ILogger<DbInitializer>>();
                    DbInitializer.Initialize(context, userManager, roleManager, dbInitializerLogger).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError("An error occurred while seeding the database.", ex);
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-CA"),
                new CultureInfo("fr-CA"),
                new CultureInfo("en-US")
            };

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-CA"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                ApplyCurrentCultureToResponseHeaders = true
            };

            var cookieProvider = localizationOptions.RequestCultureProviders
                .OfType<CookieRequestCultureProvider>()
                .First();

            cookieProvider.CookieName = "UserCulture";

            //var requestProvider = new RouteDataRequestCultureProvider();
            //localizationOptions.RequestCultureProviders.Insert(0, requestProvider);

            app.UseRequestLocalization(localizationOptions);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseTenant();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                //context.Response.Headers.Add("Strict-Transport-Security", "max-age=86400");
                context.Response.Headers.Add("Cache-Control", "no-cache");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                context.Response.Headers.Remove("X-Powered-By");

                await next();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
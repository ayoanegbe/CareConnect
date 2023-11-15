using CareConnect.CommonLogic.Interfaces;
using CareConnect.CommonLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CareConnect.CommonLogic.Services
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ITenantRepository _tenant;
        private readonly IConfiguration _config;

        public TenantResolutionMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context, ITenantSetter tenantSetter, ITenantRepository tenantRepo)
        {
            (string tenantName, string realPath) = GetTenantAndPathFrom(context.Request);
            var connectionString = _config.GetConnectionString("DefaultConnection");

            //Tenant tenant = await tenantRepo.GetTenant(tenantName);

            //if (tenant == null)
            //{
            //    //context.Response.StatusCode = 404;
            //    //return;
            //}
            //else
            //{
                //var dbConnection = new SqlConnection(connectionString);
                //Tenant tenant = await tenantRepo.GetTenant(tenantName);
                
                // Retrieve the tenant data from the database
                //using var connection = dbConnection;
                //connection.Open();
                //using var command = new SqlCommand("SELECT Name FROM Tenants WHERE Name = @Name", connection);
                //command.Parameters.AddWithValue("@Name", tenantName);
                //using var reader = command.ExecuteReader();
                //if (reader.Read())
                //{

                //    tenant.Name = reader["Name"].ToString();
                //    tenant.TenantId = int.Parse(reader["TenantId"].ToString());

                //}               

                //context.Request.PathBase = $"/{tenant.Name}";
                //context.Request.Path = realPath;
                //tenantSetter.CurrentTenant = tenant;
                
            //}

            await _next(context);
        }

        private static (string tenantName, string realPath) GetTenantAndPathFrom(HttpRequest httpRequest)
        {
            // example: https://localhost/tenant1 -> gives tenant1
            var tenantName = new Uri(httpRequest.GetDisplayUrl()).Segments.FirstOrDefault(x => x != "/")?.TrimEnd('/');

            if (!string.IsNullOrWhiteSpace(tenantName) &&
                httpRequest.Path.StartsWithSegments($"/{tenantName}", out PathString realPath))
            {
                return (tenantName, realPath);
            }

            return (null, null);
        }
    }
}

namespace CareConnect.Services
{
    public static class TenantSecurityMiddlewareExtension
    {
        public static IApplicationBuilder UseTenant(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantResolutionMiddleware>();
            return app;
        }
    }
}

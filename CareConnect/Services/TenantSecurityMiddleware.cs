namespace CareConnect.Services
{
    public class TenantSecurityMiddleware
    {
        private readonly RequestDelegate next;

        public TenantSecurityMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //string tenantIdentifier = context.Session.GetString("TenantId");
            //if (string.IsNullOrEmpty(tenantIdentifier))
            //{
            //    //var apiKey = context.Request.Headers["X-Api-Key"].FirstOrDefault();
            //    //if (string.IsNullOrEmpty(apiKey))
            //    //{
            //    //    return;
            //    //}

            //    //if (!Guid.TryParse(apiKey, out Guid apiKeyGuid))
            //    //{
            //    //    return;
            //    //}

            //    string tenantId = await tenant.GetTenantId(apiKeyGuid);
            //    context.Session.SetString("TenantId", tenantId);
            //}
            await next.Invoke(context);
        }
    }
}

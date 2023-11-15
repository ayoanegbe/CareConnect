using CareConnect.CommonLogic.Data;
using CareConnect.CommonLogic.Interfaces;
using CareConnect.CommonLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CareConnect.CommonLogic.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TenantRepository(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> GetTenantId(Guid apiKey)
        {
            try
            {
                var tenant = await _context.Tenants.Where(x => x.ApiKey.Equals(apiKey)).FirstOrDefaultAsync();
                return tenant.TenantId.ToString();
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> GetTenantId()
        {
            return await Task.FromResult(Session.GetString("TenantId"));
        }

        public async Task<string> GetTenantId(string tenantName)
        {
            return await Task.FromResult(Session.GetString(tenantName));
        }

        public async Task<string> GetTenantName(Guid tenantId)
        {
            try
            {
                var tenant = await _context.Tenants.Where(x => x.TenantId.Equals(tenantId)).FirstOrDefaultAsync();
                return tenant.Name;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IsTenant(string tenantName)
        {
            return await _context.Tenants.AnyAsync(x => x.Name.Equals(tenantName));
        }

        public async Task<Tenant> GetTenant(string tenantName)
        {
            return await _context.Tenants.FirstOrDefaultAsync(x => x.Name.Equals(tenantName));
        }

        public async Task<Tenant> AddDefaultTenant()
        {
            string tenantName = _configuration.GetValue<string>("DefaultTenant");

            if (tenantName == null) { return null; }

            Tenant tenant = new() 
            { 
                Name = "Support",
                ApiKey = Guid.NewGuid(),
                DateAdded = DateTime.Now,
                AddedBy = "System"
            };

            if (!_context.Tenants.Any())
            {
                await _context.AddAsync(tenant);
                await _context.SaveChangesAsync();
                return tenant;
            }

            return null;
        }
    }
}

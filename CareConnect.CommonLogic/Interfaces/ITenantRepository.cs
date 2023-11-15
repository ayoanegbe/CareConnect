using CareConnect.CommonLogic.Models;

namespace CareConnect.CommonLogic.Interfaces
{
    public interface ITenantRepository
    {

        Task<string> GetTenantId(Guid apiKey);
        Task<string> GetTenantId();
        Task<string> GetTenantId(string tenantName);
        Task<string> GetTenantName(Guid tenantId);
        Task<bool> IsTenant(string tenantName);
        Task<Tenant> GetTenant(string tenantName);
        Task<Tenant> AddDefaultTenant();
    }
}

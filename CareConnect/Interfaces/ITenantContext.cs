using CareConnect.Models;

namespace CareConnect.Interfaces
{
    public interface ITenantContext
    {
        Tenant CurrentTenant { get; }
    }
}

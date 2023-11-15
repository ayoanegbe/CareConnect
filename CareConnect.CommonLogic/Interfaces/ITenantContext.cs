using CareConnect.CommonLogic.Models;

namespace CareConnect.CommonLogic.Interfaces
{
    public interface ITenantContext
    {
        Tenant CurrentTenant { get; }
    }
}

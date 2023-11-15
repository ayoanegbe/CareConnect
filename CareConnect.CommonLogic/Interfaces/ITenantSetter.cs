using CareConnect.CommonLogic.Models;

namespace CareConnect.CommonLogic.Interfaces
{
    public interface ITenantSetter
    {
        Tenant CurrentTenant { set; }
    }
}

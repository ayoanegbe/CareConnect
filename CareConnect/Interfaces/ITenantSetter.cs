using CareConnect.Models;

namespace CareConnect.Interfaces
{
    public interface ITenantSetter
    {
        Tenant CurrentTenant { set; }
    }
}

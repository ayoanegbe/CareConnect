using CareConnect.CommonLogic.Interfaces;

namespace CareConnect.CommonLogic.Models
{
    public class TenantContext : ITenantContext, ITenantSetter
    {
        public Tenant CurrentTenant { get; set; }
    }
}

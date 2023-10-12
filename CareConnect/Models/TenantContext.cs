using CareConnect.Interfaces;

namespace CareConnect.Models
{
    public class TenantContext : ITenantContext, ITenantSetter
    {
        public Tenant CurrentTenant { get; set; }
    }
}

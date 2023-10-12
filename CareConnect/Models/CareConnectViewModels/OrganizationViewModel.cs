using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models.CareConnectViewModels
{
    public class OrganizationViewModel
    {
        public int OrganizationId { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

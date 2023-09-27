using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }
        [ForeignKey("Organization_Tenant")]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; } = true;
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateAssigned { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

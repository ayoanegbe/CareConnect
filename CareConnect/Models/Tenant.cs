using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models
{
    public class Tenant
    {
        [Key]
        public int TenantId { get; set; }
        [Required]
        public Guid ApiKey { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
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

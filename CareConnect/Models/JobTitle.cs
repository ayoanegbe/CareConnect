using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class JobTitle
    {
        [Key]
        public int JobTitleId { get; set; }
        [ForeignKey("JobTitle_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

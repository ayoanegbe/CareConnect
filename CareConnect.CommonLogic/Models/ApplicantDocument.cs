using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models
{
    public class ApplicantDocument
    {
        [Key]
        public int ApplicantDocumentId { get; set; }
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        [Required]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }
        public string FilePath { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

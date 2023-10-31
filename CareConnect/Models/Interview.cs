using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class Interview
    {
        [Key]
        public int InterviewId { get; set; }
        [ForeignKey("Interview_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Interview_Applicant")]
        [Display(Name = "Applicant")]
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Interview Date")]
        public DateTime InterviewDate { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Interview Time")]
        public DateTime InterviewTime { get; set;}
        public string Note { get; set; }
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

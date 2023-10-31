using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models
{
    public class Interviewer
    {
        [Key]
        public int InterviewerId { get; set; }
        public int InterviewId { get; set; }
        public Interview Interview { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public bool IsAvailable { get; set; } = true;
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

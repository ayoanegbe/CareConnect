using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class JobTitleHistory
    {
        [Key]
        public int JobTitleHistoryId { get; set; }
        [ForeignKey("JobTitleHistory_JobTitle")]
        public int JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }
        [ForeignKey("JobTitleHistory_Employee")]
        public int EmployerId { get; set; }
        public Employee Employee { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set;}
    }
}

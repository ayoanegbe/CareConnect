using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareConnect.Enums;

namespace CareConnect.Models
{
    public class EmployeePreference
    {
        [Key]
        public int PreferenceId { get; set; }
        [ForeignKey("EmployeePreference_Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [ForeignKey("EmployeePreference_ShiftPattern")]
        public int ShiftPatternId { get; set; }
        public ShiftPattern ShiftPattern { get; set; }
        public Priority Priority { get; set; } = Priority.Low;
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareConnect.CommonLogic.Enums;

namespace CareConnect.CommonLogic.Models
{
    public class EmployeeOvertime
    {
        [Key]
        public int EmployeeOvertimeId { get; set; }
        [ForeignKey("EmployeeOvertime_Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [Required]
        [Display(Name = "Overtime Hours Worked")]
        public double OvertimeHoursWorked { get; set; }
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
        [Display(Name = "Reject Reason")]
        public string RejectReason { get; set; }
        [Display(Name = "Approval By")]
        public string ApprovalBy { get; set; }
        public List<OvertimeComment> Comments { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

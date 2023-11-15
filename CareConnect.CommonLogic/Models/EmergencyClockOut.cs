using CareConnect.CommonLogic.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class EmergencyClockOut
    {
        [Key]
        public int EmergencyClockOutId { get; set; }
        [ForeignKey("EmergencyClockOut_EmployeeAttendance")]
        public int EmployeeAttendanceId { get; set; }
        public EmployeeAttendance EmployeeAttendance { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Required]
        public string Reason { get; set; }
        [ForeignKey("EmergencyClockOut_CaseManager")]
        public int CaseManagerId { get; set; }
        public CaseManager CaseManager { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        [Display(Name = "Reject Reason")]
        public string RejectReason { get; set; }
        [Display(Name = "Approval Date")]
        public DateTime? ApprovalDate { get; set; }
    }
}

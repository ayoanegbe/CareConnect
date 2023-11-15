using CareConnect.CommonLogic.Enums;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models
{
    public class ShiftSwapRequest
    {
        [Key]
        public int SwapRequestId { get; set; }
        public int OriginalShiftId { get; set; }
        public int RequestedShiftId { get; set; }
        public Shift Shift { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; } = DateTime.Now;
        [Display(Name = "Approval Status")]
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        [Display(Name = "Rejection Reason")]
        public string RejectReason { get; set; }
        [Display(Name = "Approval Date")]
        public DateTime? ApprovalDate { get; set; }
        [Display(Name = "Approval By")]
        public string ApprovedBy { get; set; }
    }
}

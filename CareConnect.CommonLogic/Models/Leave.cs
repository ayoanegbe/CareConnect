using CareConnect.CommonLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareConnect.CommonLogic.Models
{
    public class Leave
    {
        [Key]
        public int LeaveId { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int LeaveSettingId { get; set; }
        public LeaveSetting LeaveSetting { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Number Of Days")]
        public int NumberOfDays { get; set; }
        [Display(Name = "Leave Reason")]
        public string LeaveReason { get; set; }
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
        [Display(Name = "Reject Reason")]
        public string RejectReason { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        [Display(Name = "Date Approved")]
        public DateTime DateApproved { get; set; }
        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }
    }
}

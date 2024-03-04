using CareConnect.CommonLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class LeaveViewModel
    {
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
        public int LeaveDaysTaken { get; set; }
        public int LeaveDaysRemaining { get; set; }
        public List<LeaveSetting> LeaveSettings { get; set;}
    }
}

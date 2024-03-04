using CareConnect.CommonLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class LeaveSettingViewModel
    {
        public int LeaveSettingId { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Display(Name = "Pay Grade")]
        public int PayGradeId { get; set; }
        public PayGrade PayGrade { get; set; }
        [Required]
        [Display(Name = "Leave Type")]
        public LeaveType LeaveType { get; set; }
        [Display(Name = "Leave Days")]
        public int LeaveDays { get; set; }
        [Display(Name = "Carry Forward?")]
        public bool IsCarryForward { get; set; } = false;
        [Display(Name = "Max Carry Forward")]
        public int MaxCarryForward { get; set; }
        [Display(Name = "Paid Leave?")]
        public bool IsPaidLeave { get; set; } = false;
        public bool IsEdit { get; set; } = false;
    }
}

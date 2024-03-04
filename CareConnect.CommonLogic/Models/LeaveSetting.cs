using CareConnect.CommonLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareConnect.CommonLogic.Models
{
    public class LeaveSetting
    {
        [Key]
        public int LeaveSettingId { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Display(Name = "Pay Grade")]
        public int PayGradeId { get; set; }
        public PayGrade PayGrade { get; set; }
        [Display(Name = "Leave Type")]
        public LeaveType LeaveType { get; set; }
        public int LeaveDays { get; set; }
        [Display(Name = "Carry Forward?")]
        public bool IsCarryForward { get; set; }
        [Display(Name = "Max Carry Forward")]
        public int MaxCarryForward { get; set; }
        [Display(Name = "Paid Leave?")]
        public bool IsPaidLeave { get; set; }
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

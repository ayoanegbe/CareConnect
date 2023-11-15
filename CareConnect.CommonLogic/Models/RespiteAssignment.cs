using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class RespiteAssignment
    {
        [Key]
        public int RespiteAssignmentId { get; set; }
        [ForeignKey("RespiteAssignment_Respite")]
        public int RespiteId { get; set; }
        public Respite Respite { get; set; }
        [ForeignKey("RespiteAssignment_Employee")]
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; } = null;
        [Display(Name = "Date Assigned")]
        public DateTime DateAssigned { get; set; } = DateTime.UtcNow;
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Declined")]
        public bool IsDeclined { get; set; } = false;
        public DateTime? DateDeclined { get; set; }
        [Display(Name = "De-Assigned")]
        public bool DeAssign { get; set; } = false;
        [Display(Name = "Date De-Assigned")]
        public DateTime? DateDeassigned { get; set; }
        [Display(Name = "De-Assigned By")]
        public string DeAssignedBy { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class EmployeeDeductionsMap
    {
        [Key]
        public int EmployeeDeductionsMapId { get; set; }
        [ForeignKey("EmployeeDeductionsMap_Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [ForeignKey("EmployeeDeductionsMap_PayGradeLevelDeduction")]
        public int PayGradeLevelDeductionId { get; set; }
        public PayGradeLevelDeduction PayGradeLevelDeduction { get; set; }
        [Display(Name = "Date Assigned")]
        public DateTime DateAssigned { get; set; } = DateTime.Now;
        [Display(Name = "Assigned By")]
        public string AssignedBy { get; set; }
        [Display(Name = "De-Assigned")]
        public bool DeAssign { get; set; } = false;
        [Display(Name = "Date De-Assigned")]
        public DateTime? DateDeassigned { get; set; }
        [Display(Name = "De-Assigned By")]
        public string DeAssignedBy { get; set; }
    }
}

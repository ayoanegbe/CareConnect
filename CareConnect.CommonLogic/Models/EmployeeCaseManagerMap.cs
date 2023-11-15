using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class EmployeeCaseManagerMap
    {
        [Key]
        public int EmployeeCaseManagerMapId { get; set; }
        [ForeignKey("EmployeeCaseManagerMap_CaseManager")]
        public int CaseManagerId { get; set; }
        public CaseManager CaseManager { get; set; }
        [ForeignKey("EmployeeCaseManagerMap_Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
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

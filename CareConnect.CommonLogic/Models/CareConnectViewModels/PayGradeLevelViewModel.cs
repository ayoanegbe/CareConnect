using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class PayGradeLevelViewModel
    {
        public int PayGradeLevelId { get; set; }
        [ForeignKey("PayGradeLevel_PayGrade")]
        [Display(Name = "Pay Grade")]
        public int PayGradeId { get; set; }
        public PayGrade PayGrade { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1, 9)]
        public int Level { get; set; } = 1;
        [Display(Name = "Basic Salary")]
        public double BasicSalary { get; set; }
        [Display(Name = "Hourly Rate")]
        public double? HourlyRate { get; set; }
        [Display(Name = "Overtime Rate")]
        public double? OvertimeRate { get; set; }
        [ForeignKey("PayGradeLevel_Currency")]
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}

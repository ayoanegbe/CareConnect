using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class PayGradeLevelDeduction
    {
        [Key]
        public int PayGradeLevelDeductionId { get; set; }
        [ForeignKey("PayGradeLevelDeduction_PayGradeLevel")]
        public int PayGradeLevelId { get; set; }
        public PayGradeLevel PayGradeLevel { get; set; }
        [Display(Name = "Tax Rate")]
        public double TaxRate { get; set; }
        [Display(Name = "Health Insurance")]
        public double HealthInsurance { get; set; }
        [Display(Name = "Retirement Contribution")]
        public double RetirementContribution { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

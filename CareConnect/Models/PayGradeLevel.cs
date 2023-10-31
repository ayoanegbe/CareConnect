using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{

    public class PayGradeLevel
    {
        [Key]
        public int PayGradeLevelId { get; set; }
        [ForeignKey("PayGradeLevel_PayGrade")]
        [Display(Name = "Pay Grade")]
        public int PayGradeId { get; set; }
        public PayGrade PayGrade { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1, 9)]
        public int Level { get; set; }
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

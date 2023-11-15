using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models
{
    public class Deduction
    {
        [Key]
        public int DeductionId { get; set; }
        public int PayGradeLevelId { get; set; }
        public PayGradeLevel PayGradeLevel { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models
{
    public class PayrollDeduction
    {
        [Key]
        public string PayrollDeductionId { get; set; }
        public int PayrollId { get; set; }
        public Payroll Payroll { get; set; }
        public int DeductionId { get; set; }
        public Deduction Deduction { get; set; }
    }
}

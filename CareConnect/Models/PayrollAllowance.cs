using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models
{
    public class PayrollAllowance
    {
        [Key]
        public string PayrollAllowanceId { get; set; }
        public int PayrollId { get; set; }
        public Payroll Payroll { get; set; }
        public int AllowanceId { get; set; }
        public Allowance Allowance { get; set; }
    }
}

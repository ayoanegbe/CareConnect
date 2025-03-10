﻿using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models
{
    public class Payroll
    {
        [Key]
        public int PayrollId { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Required]
        [Display(Name = "Pay Period Start Date")]
        [DataType(DataType.Date)]
        public DateTime PayPeriodStartDate { get; set; }
        [Required]
        [Display(Name = "Pay Period End Date")]
        [DataType(DataType.Date)]
        public DateTime PayPeriodEndDate { get; set; }
        [Required]
        [Display(Name = "Regular Hours Worked")]
        public double RegularHoursWorked { get; set; }
        [Display(Name = "Overtime Hours Worked")]
        public double OvertimeHoursWorked { get; set; }
        [Required]
        [Display(Name = "Gross Pay")]
        public double GrossPay { get; set; }
        [Display(Name = "Total Deduction")]
        public double? TotalDeduction { get; set; }
        [Display(Name = "Net Pay")]
        public double NetPay { get; set; }
        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }
    }
}

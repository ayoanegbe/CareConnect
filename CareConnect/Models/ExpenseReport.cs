using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class ExpenseReport
    {
        [Key]
        public int ExpenseReportId { get; set; }
        [ForeignKey("ExpenseReport_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("ExpenseReport_Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [ForeignKey("ExpenseReport_Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime? Date { get; set; }
        public double? CashIn { get; set; }
        public double? CashOut { get; set;}
        public double Balance { get; set;} = 0.00;
    }
}

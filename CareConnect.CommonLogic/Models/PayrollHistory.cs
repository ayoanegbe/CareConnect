using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class PayrollHistory
    {
        [Key]
        public int PayrollHistoryId { get; set; }
        public int PayrollId { get; set; }
        public Payroll Payroll { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        
    }
}

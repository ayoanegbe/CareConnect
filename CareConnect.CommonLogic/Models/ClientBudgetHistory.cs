using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class ClientBudgetHistory
    {
        [Key]
        public int ClientBudgetHistoryId { get; set; }
        [ForeignKey("ClientBudgetHistory_Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public double Amount { get; set; }
        [ForeignKey("ClientBudgetHistory_Currency")]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        [Display(Name = "Budget Start Date")]
        public DateTime BudgetStartDate { get; set; }
        [Display(Name = "Budget End Date")]
        public DateTime BudgetEndDate { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;        
    }
}

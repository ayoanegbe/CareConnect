using CareConnect.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class SubscriptionRate
    {
        [Key]
        public int SubscriptionRateId { get; set; }
        [Required]
        public SubscriptionType Type { get; set; } = SubscriptionType.Demo;
        [Required]
        public SubscriptionPeriod Period { get; set; } = SubscriptionPeriod.Monthly;
        [Required]
        public double Amount { get; set; }
        [ForeignKey("SubscriptionRate_Currency")]
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        [Display(Name = "Number Of Employees")]
        public int? NumberOfEmployees { get; set; }
        [Display(Name = "Number Of Clients")]
        public int? NumberOfClients { get; set; }

        public string Description => $"{Type} - {Period}";
    }
}

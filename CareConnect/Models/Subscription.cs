using CareConnect.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }
        [ForeignKey("Subscription_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Subscription_SubscriptionRate")]
        public int SubscriptionRateId { get; set; }
        public SubscriptionRate SubscriptionRate { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Start Date")]
        public DateTime EndDate { get; set; }        
        public SubscriptionStatus Status { get; set; }
    }
}

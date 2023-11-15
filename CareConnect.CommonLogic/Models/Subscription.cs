using CareConnect.CommonLogic.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }
        [ForeignKey("Subscription_Organization")]
        [Display(Name = "Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Subscription_SubscriptionRate")]
        [Display(Name = "Subscription Rate")]
        public int SubscriptionRateId { get; set; }
        public SubscriptionRate SubscriptionRate { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
        [Display(Name = "Auto Renewal?")]
        public bool AutoRenewal { get; set; } = false;
    }
}

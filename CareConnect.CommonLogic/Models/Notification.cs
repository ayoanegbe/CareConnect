using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Required]
        [DataType(DataType.Html)]
        public string Message {  get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
    }
}

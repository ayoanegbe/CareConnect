using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class EmployeeAvailability
    {
        [Key]
        public int AvailabilityId { get; set; }
        [ForeignKey("EmployeeAvailability_Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }        
        [Required]
        [Display(Name = "Date Available")]
        [DataType(DataType.Date)]
        public DateTime DateAvailable { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime StartTime {  get; set; }
        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class OvertimeComment
    {
        [Key]
        public int OvertimeCommentId { get; set; }
        [ForeignKey("OvertimeComment_Overtime")]
        public int EmployeeOvertimeId { get; set; }
        public EmployeeOvertime Overtime { get; set; }
        [Required]
        public string Comment { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}

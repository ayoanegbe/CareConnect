using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class EmployeeAttendance
    {
        [Key]
        public int EmployeeAttendanceId { get; set; }
        [ForeignKey("EmployeeAttendance_ShiftAssigment")]
        public int ShiftAssigmentId { get; set; }
        public ShiftAssigment ShiftAssigment { get; set; }
        [Display(Name = "Clock In")]
        public DateTime ClockIn { get; set; }
        [Display(Name = "Clock Out")]
        public DateTime? ClockOut { get; set; }
        [Display(Name = "Hours Worked")]
        public double HoursWorked { get; set; }

    }
}

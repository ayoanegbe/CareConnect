using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class HourlyTimeSheet
    {
        [Key]
        public int HourlyTimeSheetId { get; set; }
        [ForeignKey("HourlyTimeSheet_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("HourlyTimeSheet_Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [ForeignKey("HourlyTimeSheet_Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public int? Hours { get; set; }
        public double? Kilometer { get; set; }
        public string Activities { get; set; }
    }
}

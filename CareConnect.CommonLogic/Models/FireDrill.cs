using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class FireDrill
    {
        [Key]
        public int FireDrillId { get; set; }
        [ForeignKey("FireDrill_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("FireDrill_Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [ForeignKey("FireDrill_Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [ForeignKey("FireDrill_CaseManager")]
        public int CaseManagerId { get; set; }
        public CaseManager CaseManager { get; set; }
        public DateTime? DrillDate { get; set; }
        [DataType(DataType.Time)]
        public DateTime? DrillTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime? TimeToEvacuate { get; set; }
        public bool DetectorActivated { get; set; }
        public bool FireLocationIndicated { get; set; }
        public bool ExitPointsIndicated { get; set; }
        public bool Contacted911 { get; set; }
        public bool WindowsAndDoorsClosed { get; set; }
        public bool PersonalInformationTaken { get; set; }
        public bool EveryoneGetOutSafely { get; set; }
        public bool HeadCountTaken { get; set; }
        public string MeetingPoint { get; set; }
        public string NaturalSupports { get; set; }
        [DataType(DataType.Date)]
        public DateTime AlarmsCheckDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime FireExtinguisherMaintenance { get; set; }
        public bool EvacuationPlanPosted { get; set; }
        public string Where { get; set; }
        [DataType(DataType.Html)]
        public string Comment { get; set; }
    }
}

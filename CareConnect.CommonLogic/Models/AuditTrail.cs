using CareConnect.CommonLogic.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class AuditTrail
    {
        [Key]
        public int AuditTrailId { get; set; }
        [ForeignKey("AuditTrail_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string TableName { get; set; }
        public UpdateAction Action { get; set; } = UpdateAction.Update;
        public string OldValue {  get; set; } = string.Empty;
        public string NewValue { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;
        public string ChangedBy { get; set; }
    }
}

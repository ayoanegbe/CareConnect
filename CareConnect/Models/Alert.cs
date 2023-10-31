using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }
        [ForeignKey("Alert_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required]
        [StringLength(50)]
        public string Header { get; set; }
        [DataType(DataType.Html)]
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Owner { get; set; }
    }
}

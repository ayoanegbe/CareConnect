using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace CareConnect.Models
{
    public class SmtpSetting
    {
        [Key]
        public int SmtpSettingId { get; set; }
        [ForeignKey("SmtpSetting_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Host { get; set; }
        [Display(Name = "Host IP")]
        public string HostIP { get; set; }
        [Display(Name = "Port Number")]
        public string PortNumber { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        public string CcEmail { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

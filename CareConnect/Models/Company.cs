using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace CareConnect.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public List<House> Houses { get; set; }
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

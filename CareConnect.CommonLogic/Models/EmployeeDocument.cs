using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace CareConnect.CommonLogic.Models
{
    public class EmployeeDocument
    {
        [Key]
        public int EmployeeDocumentId { get; set; }
        [ForeignKey("EmployeeDocument_Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Required]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }
        public string FilePath { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

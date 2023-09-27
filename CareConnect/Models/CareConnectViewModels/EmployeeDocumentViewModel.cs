using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models.CareConnectViewModels
{
    public class EmployeeDocumentViewModel
    {
        public int EmployeeDocumentId { get; set; }
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
        public IFormFile File { get; set; }
    }
}

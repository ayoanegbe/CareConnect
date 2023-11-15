using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class EmployeeDocumentViewModel
    {
        public int EmployeeDocumentId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Required]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }
        [Display(Name = "File Path")]
        public string FilePath { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}

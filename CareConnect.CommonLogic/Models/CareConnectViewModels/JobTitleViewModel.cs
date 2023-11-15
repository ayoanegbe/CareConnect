using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class JobTitleViewModel
    {
        public int JobTitleId { get; set; }
        [ForeignKey("JobTitle_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

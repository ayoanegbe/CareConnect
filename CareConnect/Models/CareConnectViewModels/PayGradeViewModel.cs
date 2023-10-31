using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models.CareConnectViewModels
{
    public class PayGradeViewModel
    {
        public int PayGradeId { get; set; }
        [ForeignKey("PayGrade_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

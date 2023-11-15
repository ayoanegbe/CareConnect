using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.CommonLogic.Models
{
    public class ShiftPattern
    {
        [Key]
        public int ShiftPatternId { get; set; }
        [ForeignKey("ShiftPattern_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required]
        [Display(Name = "Pattern Name")]
        public string PatternName { get; set;}
        [Display(Name = "Pattern Description")]
        public string PatternDescription { get; set;}
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set;}
    }
}

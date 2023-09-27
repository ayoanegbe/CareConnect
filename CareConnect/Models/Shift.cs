using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareConnect.Enums;

namespace CareConnect.Models
{
    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }
        [ForeignKey("Shift_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Shift_ShiftPattern")]
        public int ShiftPatternId { get; set; }
        public ShiftPattern ShiftPattern { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Perpetual?")]
        public bool Perpetual { get; set; } = false;
        [DataType(DataType.Html)]
        public string Note { get; set; }
        public bool IsAssigned { get; set; } = false;
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Range(1, 5)]
        [Display(Name = "Numbers Required")]
        public int NumbersRequired { get; set; } = 1;
        public int? NumbersAssigned { get; set; }
        [ForeignKey("Shift_Client")]
        public int? ClientId { get; set; }
        public Client Client { get; set; } = null;
        [ForeignKey("Shift_House")]
        public int? HouseId { get; set; }
        public House House { get; set; } = null;
        public ShiftAssigment ShiftAssigment { get; set; }
    }
}

using CareConnect.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class Respite
    {
        [Key]
        public int RespiteId { get; set; }
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
        [ForeignKey("Respite_Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [Display(Name = "Respite Type")]
        public RespiteType RespiteType { get; set; } = RespiteType.Employee;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class ShiftViewModel
    {
        public int ShiftId { get; set; }
        [ForeignKey("Shift_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Shift_ShiftPattern")]
        [Display(Name = "Shift Pattern")]
        public int ShiftPatternId { get; set; }
        public ShiftPattern ShiftPattern { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        [Range(1, 10)]
        [Display(Name = "Numbers Required")]
        public int NumbersRequired { get; set; } = 1;
        [Display(Name = "Repeat?")]
        public bool Perpetual { get; set; } = false;
        [Display(Name = "Sun")]
        public bool Sunday { get; set; }
        [Display(Name = "Mon")]
        public bool Monday { get; set; }
        [Display(Name = "Tue")]
        public bool Tuesday { get; set; }
        [Display(Name = "Wed")]
        public bool Wednesday { get; set; }
        [Display(Name = "Thu")]
        public bool Thursday { get; set; }
        [Display(Name = "Fri")]
        public bool Friday { get; set; }
        [Display(Name = "Sat")]
        public bool Saturday { get; set; }
        [DataType(DataType.Html)]
        [Required]
        public string Note { get; set; }
        [ForeignKey("Shift_Client")]
        [Display(Name = "Client")]
        public int? ClientId { get; set; }
        public Client Client { get; set; } = null;
        [ForeignKey("Shift_House")]
        [Display(Name = "House")]
        public int? HouseId { get; set; }
        public House House { get; set; } = null;
    }
}

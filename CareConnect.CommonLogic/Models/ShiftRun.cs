using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareConnect.CommonLogic.Models
{
    public class ShiftRun
    {
        [Key]
        public int ShiftRunId { get; set; }
        public int ShiftId { get; set; }
        public Shift Shift { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Shift Date")]
        public DateTime ShiftDate { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Shift Time")]
        public DateTime ShiftTime { get; set; }
        [Display(Name = "Fully Assigned")]
        public bool IsAssigned { get; set; } = false;        
        [Display(Name = "# Assigned")]
        public int? NumbersAssigned { get; set; }
        public ShiftAssigment ShiftAssigment { get; set; }
    }
}

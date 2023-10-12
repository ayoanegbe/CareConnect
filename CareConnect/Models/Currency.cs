using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }
        [Required]
        public string Code { get; set;}
        [Required]
        public string Name { get; set;}
        public string Symbols { get; set;} = string.Empty;
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

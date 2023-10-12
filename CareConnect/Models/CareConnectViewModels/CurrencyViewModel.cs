using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models.CareConnectViewModels
{
    public class CurrencyViewModel
    {
        public int CurrencyId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Symbols { get; set; } = string.Empty;
    }
}

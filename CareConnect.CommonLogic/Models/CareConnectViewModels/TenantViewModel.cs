using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class TenantViewModel
    {
        public int TenantId { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}

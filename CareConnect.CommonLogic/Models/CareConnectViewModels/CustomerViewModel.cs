using CareConnect.CommonLogic.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        [Display(Name = "Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required]
        [Display(Name = "Customer Type")]
        public CustomerType CustomerType { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "Contact Person's Name")]
        public string ContactPersonName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Person's Phone")]
        public string ContactPersonPhone { get; set; }
        [DataType(DataType.Html)]
        public string Notes { get; set; }
        public string Comment { get; set; }
    }
}

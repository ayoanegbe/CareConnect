using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models.CareConnectViewModels
{
    public class HouseViewModel
    {
        public int HouseId { get; set; }
        [Display(Name = "House Name")]
        public string HouseName { get; set; }
        [Display(Name = "Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; } = true;
    }
}

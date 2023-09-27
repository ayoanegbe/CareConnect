using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class House
    {
        [Key]
        public int HouseId { get; set; }
        [Display(Name = "House Name")]
        public string HouseName { get; set; }
        [ForeignKey("House_Organization")]
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
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}

using CareConnect.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [ForeignKey("Customer_Organization")]
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
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        public string Comment { get; set; }
    }
}

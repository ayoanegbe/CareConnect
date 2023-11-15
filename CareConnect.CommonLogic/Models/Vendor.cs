using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareConnect.CommonLogic.Enums;

namespace CareConnect.CommonLogic.Models
{
    public class Vendor
    {
        [Key]
        public int VendorId { get; set; }
        [ForeignKey("Vendor_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public string City { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        #region Bank Details
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.DirectDeposit;
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Bank Code")]
        public string BankCode { get; set; }
        [Display(Name = "Transit Code")]
        public string TransitCode { get; set; }
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        #endregion

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

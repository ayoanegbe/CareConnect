using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using CareConnect.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CareConnect.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [ForeignKey("Employee_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Employee_Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Employee_JobTitle")]
        public int JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public Gender Gender { get; set; } = Gender.Female;
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Address { get; set; }
        public string City { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Emergency Contact Phone")]
        public string EmergencyContactPhone { get; set; }
        [Display(Name = "Emergency Contact Address")]
        public string EmergencyContactAddress { get; set; }
        [Display(Name = "Relationship To Employee")]
        public string Relationship { get; set; }
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; } = true;
        [Display(Name = "Date Joined")]
        public DateTime? DateJoined { get; set; }
        public EmployeeType Status { get; set; } = EmployeeType.Contract;
        [Display(Name = "Contractor Type")]
        public ContractorType? ContractorType { get; set; }
        [Display(Name = "Immigration Status")]
        public ImmigrationStatus ImmigrationStatus { get; set; } = ImmigrationStatus.Citizen;
        [Display(Name = "Social Insurance Number")]
        public string SocialInsuranceNumber { get; set; }
        [Display(Name = "Line Manager")]
        public int? LineManagerId { get; set; }

        #region Bank Details
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.DirectDeposit;
        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Required]
        [Display(Name = "Bank Code")]
        public string BankCode { get; set; }
        [Required]
        [Display(Name = "Transit Code")]
        public string TransitCode { get; set; }
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; } = PaymentFrequency.Monthly;
        #endregion

        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        public string FullName => $"{this.LastName}, {this.FirstName} {this.MiddleName}";
    }
}

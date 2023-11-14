using CareConnect.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models.CareConnectViewModels
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        [ForeignKey("Employee_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Employee_Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Employee_JobTitle")]
        [Display(Name = "Job Title")]
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
        [Display(Name = "Contract Type")]
        public ContractorType? ContractorType { get; set; }
        [Display(Name = "Immigration Status")]
        public ImmigrationStatus ImmigrationStatus { get; set; } = ImmigrationStatus.Citizen;
        [Display(Name = "Social Insurance Number")]
        public string SocialInsuranceNumber { get; set; }
        [Display(Name = "Line Manager")]
        public int? LineManagerId { get; set; }
        [Display(Name = "Pay Grade Level")]
        public int PayGradeLevelId { get; set; }
        public PayGradeLevel PayGradeLevel { get; set; }

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
        public PaymentFrequency PaymentFrequency { get; set; } = PaymentFrequency.Monthly;
        #endregion

        public List<EmployeeDocument> Documents { get; set; }
        [Display(Name = "Full Name")]
        public string FullName => $"{this.LastName}, {this.FirstName} {this.MiddleName}";
    }
}

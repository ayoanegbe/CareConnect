using CareConnect.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        [ForeignKey("Client_Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("Client_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int? HouseId { get; set; }
        public House House { get; set; } = null;
        [Required]
        [Display(Name = "Residential Type")]
        public ResidentialType ResidentialType { get; set; } = ResidentialType.GroupHome;
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Display(Name = "Date Joined")]
        [DataType(DataType.Date)]
        public DateTime DateJoined { get; set; }
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Emergency Contact Phone")]
        public string EmergencyContactPhone { get; set; }
        [Display(Name = "Emergency Contact Address")]
        public string EmergencyContactAddress { get; set; }
        [Display(Name = "Relationship To Client")]
        public RelationshipType Relationship { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Guadian Phone Number")]
        public string GuadianPhoneNumber { get; set; }
        [Display(Name = "Family Physician")]
        public string FamilyPhysician { get; set; }
        public bool IsActive { get; set; } = true;
        public string Psychiatrist { get; set; }
        public double Budget { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        [Display(Name = "Budget Start Date")]
        public DateTime BudgetStartDate { get; set; }
        [Display(Name = "Budget End Date")]
        public DateTime BudgetEndDate { get;set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        [DataType(DataType.Html)]
        public string Notes { get; set; }
        public string Comment { get; set; }

        public string FullName => $"{this.LastName}, {this.FirstName} {this.MiddleName}";
    }
}

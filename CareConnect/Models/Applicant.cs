using CareConnect.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class Applicant
    {
        [Key]
        public int ApplicantId { get; set; }
        [ForeignKey("Applicant_Vacancy")]
        [Display(Name = "Vacancy")]
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set;}
        [ForeignKey("Applicant_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email {  get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Display(Name = "Resume")]
        public string ResumePath { get; set; }
        [Display(Name = "Cover Letter")]
        public string CoverLetterPath { get; set; }
        [Display(Name = "Date Applied")]
        public DateTime DateApplied { get; set; } = DateTime.UtcNow;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Fresh;
        [Display(Name = "Full Name")]
        public string FullName => $"{this.LastName}, {this.FirstName} {this.MiddleName}";
    }
}

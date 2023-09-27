using CareConnect.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareConnect.Models
{
    public class Vacancy
    {
        [Key]
        public int VacancyId { get; set; }
        [ForeignKey("Vacancy_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Vacancy_JobTitle")]
        public int JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }
        [ForeignKey("Vacancy_Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        [Required]
        [DataType(DataType.Html)]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        [Required]
        [DataType(DataType.Html)]
        public string Requirements { get; set; }
        public VacancyStatus Status { get; set; } = VacancyStatus.Open;
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}

using CareConnect.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models.CareConnectViewModels
{
    public class VacancyViewModel
    {
        public int VacancyId { get; set; }
        [ForeignKey("Vacancy_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Vacancy_JobTitle")]
        [Display(Name = "Job Title")]
        public int JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }
        [ForeignKey("Vacancy_Department")]
        [Display(Name = "Department")]
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
        [Required]
        [Display(Name = "Closing Date")]
        public DateTime ClosingDate { get; set; }
    }
}

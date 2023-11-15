using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models.CareConnectViewModels
{
    public class InterviewViewModel
    {
        public int InterviewId { get; set; }
        [ForeignKey("Interview_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [ForeignKey("Interview_Applicant")]
        [Display(Name = "Applicant")]
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Interview Date")]
        public DateTime InterviewDate { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Interview Time")]
        public DateTime InterviewTime { get; set; }
        public string Note { get; set; }
        public string Comment { get; set; }
        public List<InterviewersList> InterviewersLists { get; set; }
        public List<Interviewer> Interviewers { get; set; }
    }
}

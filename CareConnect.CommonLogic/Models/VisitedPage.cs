using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models
{
    public class VisitedPage
    {
        [Key]
        public int VisitedPageId { get; set; }
        public string SessionUser { get; set; }
        public string PageName { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}

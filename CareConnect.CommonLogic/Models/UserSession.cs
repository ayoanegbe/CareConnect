using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models
{
    public class UserSession
    {
        [Key]
        public int UserSessionId { get; set; }
        public string SessionUser { get; set; }
        public DateTime SessionDate { get; set; } = DateTime.Now;
        public string IpAddress { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}

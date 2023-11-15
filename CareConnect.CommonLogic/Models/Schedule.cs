using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models
{
    public class Schedule
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime", NullValueHandling = NullValueHandling.Ignore)]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        public string Color { get; set; }
    }
}

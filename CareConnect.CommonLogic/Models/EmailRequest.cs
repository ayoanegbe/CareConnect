namespace CareConnect.CommonLogic.Models
{
    public class EmailRequest
    {
        public string SenderEmailAddress { get; set; }
        public string RecieverEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

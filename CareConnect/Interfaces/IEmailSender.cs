using CareConnect.Models;

namespace CareConnect.Interfaces
{
    public interface IEmailSender
    {
        Task<string> SendEmailAsync(EmailRequest request, List<string> cc = null);
        Task<string> SendEmailAsync(int organizationId, EmailRequest request, List<string> cc = null);
        Task<string> SendEmailAsync(string email, string code, string message);
    }
}

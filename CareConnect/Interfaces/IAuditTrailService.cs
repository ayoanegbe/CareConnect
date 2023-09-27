using CareConnect.Enums;
using CareConnect.Models;

namespace CareConnect.Interfaces
{
    public interface IAuditTrailService
    {
        Task<bool> UpdateAuditTrail(int OrganizationId, string className, UpdateAction action, string oldValue, string newValue, string userName);
        Task<bool> UpdateAuditTrail(int OrganizationId, string className, UpdateAction action, string newValue, string userName);
        Task<List<AuditTrail>> GetAuditTrail(int OrganizationId, string className, DateTime startDate, DateTime endDate);
        Task<(T, T)> GetAuditData<T>(int id) where T : class;
    }
}

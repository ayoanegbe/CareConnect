using CareConnect.Models;

namespace CareConnect.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<Organization> AddDefaultOrganization(int tenantId);
    }
}

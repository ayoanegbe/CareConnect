using CareConnect.CommonLogic.Models;

namespace CareConnect.CommonLogic.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<Organization> AddDefaultOrganization(int tenantId);
    }
}

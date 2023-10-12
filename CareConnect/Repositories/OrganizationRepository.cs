using CareConnect.Data;
using CareConnect.Interfaces;
using CareConnect.Models;

namespace CareConnect.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public OrganizationRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Organization> AddDefaultOrganization(int tenantId)
        {
            DefaultOrganization defOrg = _configuration.GetSection("DefaultOrganization").Get<DefaultOrganization>();

            if (defOrg == null) { return null; }

            Organization organization = new()
            {
                TenantId = tenantId,
                Name = defOrg.Name,
                Address = defOrg.Address,
                Email = defOrg.Email,
                Phone = defOrg.Phone,
                AddedBy = "System"
            };

            if (!_context.Organizations.Any())
            {
                await _context.AddAsync(organization);
                await _context.SaveChangesAsync();
                return organization;
            }

            return null;
        }
    }
}

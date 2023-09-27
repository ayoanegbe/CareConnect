using CareConnect.Controllers;
using CareConnect.Data;
using CareConnect.Enums;
using CareConnect.Interfaces;
using CareConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace CareConnect.Services
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuditTrailService> _logger;

        public AuditTrailService(ApplicationDbContext context, ILogger<AuditTrailService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> UpdateAuditTrail(int OrganizationId, string className, UpdateAction action, string oldValue, string newValue, string userName)
        {
            AuditTrail auditTrail = new()
            {
                OrganizationId = OrganizationId,
                TableName = className,
                Action = action,
                OldValue = oldValue,
                NewValue = newValue,
                ChangeDate = DateTime.UtcNow,
                ChangedBy = userName
            };

            try
            {
                await _context.AddAsync(auditTrail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"An error has occurred when trying to write audit trail for Table: {className} | Old Value: {oldValue} | New Value: {newValue}", ex);
            }

            return false;
        }

        public async Task<bool> UpdateAuditTrail(int OrganizationId, string className, UpdateAction action, string newValue, string userName)
        {
            AuditTrail auditTrail = new()
            {
                OrganizationId = OrganizationId,
                TableName = className,
                Action = action,
                NewValue = newValue,
                ChangeDate = DateTime.UtcNow,
                ChangedBy = userName
            };

            try
            {
                await _context.AddAsync(auditTrail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,
                            $"An error has occurred when trying to write audit trail for Table: {className} | New Value: {newValue}",
                            ex);
            }

            return false;
        }

        public async Task<List<AuditTrail>> GetAuditTrail(int OrganizationId, string className, DateTime startDate, DateTime endDate)
        {
            return await _context.AuditTrails.Where(x => x.OrganizationId == OrganizationId && x.TableName.Equals(className) && x.ChangeDate >= startDate && x.ChangeDate <= endDate).ToListAsync();
        }

        public async Task<(T, T)> GetAuditData<T>(int id) where T : class
        {
            try
            {
                AuditTrail auditData = await _context.AuditTrails.Where(x => x.AuditTrailId == id).FirstOrDefaultAsync();
                var oldValue = auditData.OldValue as T;
                var newValue = auditData.NewValue as T;

                return (oldValue, newValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in getting data for: {typeof(T).Name}");
            }

            return (null, null);
        }
       
    }
}

using CareConnect.CommonLogic.Data;
using CareConnect.CommonLogic.Models;
using CareConnect.CommonLogic.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareConnect.CommonLogic.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CareConnect.CommonLogic.Services
{
    public class CareConnectService : ICareConnectService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CareConnectService> _logger;

        public CareConnectService(ApplicationDbContext context,
                ILogger<CareConnectService> logger)
        {
            _context = context;
            _logger = logger;
        }
        

        public async Task SetVacancyStatus()
        {
            List<Vacancy> vacancies = await _context.Vacancies
                .Include(x => x.Organization)
                .Include(x => x.JobTitle)
                .Include(x => x.Department)
                .Where(x => x.ClosingDate < DateTime.Now)
                .ToListAsync();

            foreach (var vacancy in vacancies)
            {
               
                vacancy.Status = VacancyStatus.Closed;
                _context.SaveChanges();

                _logger.LogInformation($"{nameof(CareConnectService)} => {JsonConvert.SerializeObject(vacancy)}");
            }

            return;
        }
    }
}

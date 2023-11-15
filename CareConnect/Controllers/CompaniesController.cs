using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareConnect.CommonLogic.Data;
using CareConnect.CommonLogic.Models;
using CareConnect.CommonLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CareConnect.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly CustomIDataProtection _protector;

        public CompaniesController(ApplicationDbContext context,
            CustomIDataProtection protector,
            IConfiguration config,
            ILogger<CompaniesController> logger)
        {
            _context = context;
            _protector = protector;
            _config = config;
            _logger = logger;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        //public async Task<IActionResult> HouseIndex(string id)
        //{
        //    int num = Resolver(id);
        //    if (num == 0)
        //    {
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    var company = await _context.Companies.Where(x => x.CompanyId == num).FirstOrDefaultAsync();
        //    ViewData["company"] = company;
        //    return View(await _context.Houses.Where(x => x.CompanyId == num).ToListAsync());
        //}       

        // GET: Companies/Create
        public IActionResult Create()
        {
            var company = new Company();
            return View(company);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,ShortName,DateAdded,DateUpdated,Address,IsActive")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        public IActionResult HouseCreate(string id)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var company = _context.Companies.Where(x => x.CompanyId == num).FirstOrDefault();
            ViewData["company"] = company;
            var house = new House();
            return View(house);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> HouseCreate([Bind("HouseId,HouseName,CompanyId,DateAdded,DateUpdated,Address,IsActive,Longitude,Latitude")] string id, House house)
        //{
        //    int num = Resolver(id);
        //    if (num == 0)
        //    {
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        house.CompanyId = num;
        //        _context.Add(house);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(HouseIndex), new { id = _protector.Encode(num.ToString()) });
        //    }
        //    var company = await _context.Companies.Where(x => x.CompanyId == num).FirstOrDefaultAsync();
        //    ViewData["company"] = company;
        //    return View(house);
        //}

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var company = await _context.Companies.FindAsync(num);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CompanyId,CompanyName,ShortName,DateAdded,DateUpdated,Address,IsActive")] Company company)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != company.CompanyId)
            {
                return NotFound();
            }           

            if (ModelState.IsValid)
            {
                try
                {
                    company.DateUpdated = DateTime.UtcNow;
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        //public async Task<IActionResult> HouseEdit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    int num = Resolver(id);
        //    if (num == 0)
        //    {
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    var house = await _context.Houses.FindAsync(num);
        //    if (house == null)
        //    {
        //        return NotFound();
        //    }
        //    var company = await _context.Companies.Where(x => x.CompanyId == house.CompanyId).FirstOrDefaultAsync();
        //    ViewData["company"] = company;
        //    return View(house);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> HouseEdit(string id, int houseId, [Bind("HouseId,HouseName,CompanyId,DateAdded,DateUpdated,Address,IsActive,Longitude,Latitude")] House house)
        //{
        //    int num = Resolver(id);
        //    if (num == 0)
        //    {
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    if (houseId != house.HouseId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            house.DateUpdated = DateTime.UtcNow;
        //            _context.Update(house);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!HouseExists(house.HouseId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(HouseIndex), new { id = _protector.Encode(num.ToString()) });
        //    }
        //    var company = await _context.Companies.Where(x => x.CompanyId == house.CompanyId).FirstOrDefaultAsync();
        //    ViewData["company"] = company;
        //    return View(house);
        //}

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }

        private bool HouseExists(int id)
        {
            return _context.Houses.Any(e => e.HouseId == id);
        }

        private int Resolver(string id)
        {

            try
            {
                string decodedString = _protector.Decode(id);
                int decodedNumber = int.Parse(decodedString);
                return decodedNumber;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return 0;
            }

            
        }
    }
}

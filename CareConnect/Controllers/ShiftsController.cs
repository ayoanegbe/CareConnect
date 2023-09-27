using CareConnect.Data;
using CareConnect.Models;
using CareConnect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareConnect.Controllers
{
    [Authorize]
    public class ShiftsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly CustomIDataProtection _protector;
        private readonly ILogger<ShiftsController> _logger;

        public ShiftsController(ApplicationDbContext context,
            CustomIDataProtection protector,
            ILogger<ShiftsController> logger)
        {
            _context = context;
            _protector = protector;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Shifts.ToListAsync());
        }

        // GET: Shifts/Create
        public IActionResult Create()
        {
            Shift shift = new()
            {
                StartDate = DateTime.Now
            };

            ViewData["CompanyId"] = new SelectList(_context.Companies.Where(x => x.IsActive), "CompanyId", "CompanyName");

            return View(shift);
        }

        // POST: Shifts/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ShiftId,ShiftDate,StartTime,Duration,Note,NumbersNeeded,CompanyId,HouseId")] Shift shift)
        //{
            //ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.IsActive), "HouseId", "HouseName", shift.HouseId);

            //if (string.IsNullOrEmpty(shift.Duration))
            //{
            //    ViewBag.Message = "Kindly specify shift duration!";
            //    return View(shift);
            //}

            //int hour = int.Parse(shift.Duration.Split(":")[0]);
            //int minute = int.Parse(shift.Duration.Split(":")[1]);

            //if (minute > 59)
            //{
            //    ViewBag.Message = $"Minutes value of '{minute}' is inappropriate!";
            //    return View(shift);
            //}

            //shift.EndTime = shift.StartTime.AddHours(hour);
            //shift.EndTime = shift.EndTime.AddMinutes(minute);

            //if (shift.ShiftDate > DateTime.Today)
            //{
            //    TimeSpan diff = shift.ShiftDate.Subtract(DateTime.Today);

            //    shift.StartTime = shift.StartTime.AddDays(diff.Days);
            //    shift.EndTime = shift.EndTime.AddDays(diff.Days);
            //}            

            //if (shift.ShiftDate < DateTime.Today)
            //{
            //    ViewBag.Message = "Shift date can't be less than today!";
            //    return View(shift);
            //}

            //if (shift.ShiftDate == DateTime.Today && shift.StartTime < DateTime.Now)
            //{
            //    ViewBag.Message = "Shift start time can't be less than current time!";
            //    return View(shift);
            //}
                        
            //if (string.IsNullOrEmpty(shift.Note))
            //{
            //    ViewBag.Message = "Note field can't be empty!";
            //    return View(shift);
            //}

            //if (ModelState.IsValid)
            //{
            //    _context.Add(shift);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(HomeController.Index), "Home");
            //}

            
        //    return View(shift);
        //}

        //[HttpPost]
        //public JsonResult GetSecondData(int firstId)
        //{
        //    var result = ""; //populate result   
        //    return Json(result);
        //}

        //public JsonResult GetHouseList(int CompanyId)
        //{
        //    var data = new SelectList(_context.Houses.Where(x => x.CompanyId == CompanyId && x.IsActive), "HouseId", "HouseName");
        //    return Json(data);
        //}
    }
}

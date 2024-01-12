using CareConnect.CommonLogic.Data;
using CareConnect.CommonLogic.Enums;
using CareConnect.CommonLogic.Interfaces;
using CareConnect.CommonLogic.Models;
using CareConnect.CommonLogic.Models.CareConnectViewModels;
using CareConnect.CommonLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace CareConnect.Controllers
{
    [Authorize]
    public class ShiftManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomIDataProtection _protector;
        private readonly ILogger<ShiftManagementController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuditTrailService _audit;
        private readonly RecaptchaService _recaptchaService;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSender _emailSender;

        public ShiftManagementController(
            ILogger<ShiftManagementController> logger,
            ApplicationDbContext context,
            CustomIDataProtection protector,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuditTrailService audit,
            RecaptchaService recaptchaService,
            IFileService fileService,
            IWebHostEnvironment environment,
            IEmailSender emailSender
            )
        {
            _logger = logger;
            _context = context;
            _protector = protector;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _audit = audit;
            _recaptchaService = recaptchaService;
            _fileService = fileService;
            _environment = environment;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> ListShiftPatterns()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.ShiftPatterns
                                .Where(x => x.OrganizationId == (int)user.OrganizationId)
                                .OrderByDescending(x => x.ShiftPatternId)
                                .ToListAsync());
        }

        public async Task<IActionResult> AddShiftPattern()
        {
            var user = await _userManager.GetUserAsync(User);

            ShiftPattern shiftPattern = new() 
            { 
                OrganizationId = (int)user.OrganizationId, 
                StartTime = DateTime.ParseExact("09:00 AM", "hh:mm tt", CultureInfo.InvariantCulture),
                EndTime = DateTime.ParseExact("03:00 PM", "hh:mm tt", CultureInfo.InvariantCulture)
            };

            return View(shiftPattern);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShiftPattern([Bind("OrganizationId,PatternName,PatternDescription,StartTime,EndTime")] ShiftPattern shiftPattern)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(shiftPattern);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListShiftPatterns));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(shiftPattern);
        }

        public async Task<IActionResult> EditShiftPattern(string id)
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

            var shiftPattern = await _context.ShiftPatterns.FirstOrDefaultAsync(x => x.ShiftPatternId == num);
            if (shiftPattern == null)
            {
                return NotFound();
            }

            return View(shiftPattern);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShiftPattern(string id, [Bind("ShiftPatternId,OrganizationId,PatternName,PatternDescription,StartTime,EndTime")] ShiftPattern shiftPattern)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != shiftPattern.ShiftPatternId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shiftPattern);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ListShiftPatterns));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ShiftPatternExists(shiftPattern.ShiftPatternId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, ex, "An error has occurred fetching item");
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }

            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(shiftPattern);
        }

        public async Task<IActionResult> ListShifts()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.Shifts
                                .Include(x => x.ShiftPattern)
                                .Include(x => x.Client)
                                .Include(x => x.House)
                                .Where(x => x.OrganizationId == (int)user.OrganizationId)
                                .OrderByDescending(x => x.ShiftId)
                                .ToListAsync());

        }

        public async Task<IActionResult> AddShift()
        {
            var user = await _userManager.GetUserAsync(User);

            ShiftViewModel shift = new() { OrganizationId = (int)user.OrganizationId, StartDate = DateTime.Today, NumbersRequired = 1 };

            ViewData["ShiftPatternId"] = new SelectList(_context.ShiftPatterns.Where(x => x.OrganizationId == (int)user.OrganizationId), "ShiftPatternId", "PatternName");
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(x => x.OrganizationId == (int)user.OrganizationId), "ClientId", "FullName");
            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == (int)user.OrganizationId), "HouseId", "HouseName");

            return View(shift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShift([Bind("ShiftId,OrganizationId,ShiftPatternId,StartDate,EndDate,Perpetual,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Note,ClientId,HouseId")] ShiftViewModel shiftView)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["ShiftPatternId"] = new SelectList(_context.ShiftPatterns.Where(x => x.OrganizationId == shiftView.OrganizationId), "ShiftPatternId", "PatternName", shiftView.ShiftPatternId);
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(x => x.OrganizationId == shiftView.OrganizationId), "ClientId", "FullName", shiftView.ClientId);
            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == shiftView.OrganizationId), "HouseId", "HouseName", shiftView.HouseId);

            shiftView.ShiftPattern = await _context.ShiftPatterns.FirstOrDefaultAsync(x => x.ShiftPatternId == shiftView.ShiftPatternId);

            if (shiftView.StartDate == DateTime.Today && shiftView.ShiftPattern.StartTime.Hour < DateTime.Now.Hour)
            {
                ViewBag.Message = $"'{shiftView.ShiftPattern.PatternName}' start time is {shiftView.ShiftPattern.StartTime:hh:mm tt}. It can't be less than current time!";
                return View(shiftView);
            }

            if (shiftView.Perpetual)
            {
                if (shiftView.EndDate == null)
                {
                    ViewBag.Message = "End date is required!";
                    return View(shiftView);
                }

                if(!shiftView.Sunday && !shiftView.Monday && !shiftView.Tuesday &&  !shiftView.Wednesday && !shiftView.Thursday && !shiftView.Friday && !shiftView.Saturday)
                {
                    ViewBag.Message = "No day selected for repeated shifts";
                    return View(shiftView);
                }
            }

            if (string.IsNullOrEmpty(shiftView.Note))
            {
                ViewBag.Message = "Note field can't be empty!";
                return View(shiftView);
            }

            Shift shift = new()
            {
                OrganizationId = shiftView.OrganizationId,
                ShiftPatternId = shiftView.ShiftPatternId,
                StartDate = shiftView.StartDate,
                EndDate = shiftView.EndDate,
                Perpetual = shiftView.Perpetual,
                Sunday = shiftView.Sunday,
                Monday = shiftView.Monday,
                Tuesday = shiftView.Tuesday,
                Wednesday = shiftView.Wednesday,
                Thursday = shiftView.Thursday,
                Friday = shiftView.Friday,
                Saturday = shiftView.Saturday,
                Note = shiftView.Note,
                DateAdded = DateTime.Now,
                AddedBy = user.UserName,
                ClientId = shiftView.ClientId,
                HouseId = shiftView.HouseId,
                ShiftPattern = shiftView.ShiftPattern,
            };

            if (ModelState.IsValid)
            {
                _context.Add(shift);
                await _context.SaveChangesAsync();               

                await SetShiftRun(shift);

                return RedirectToAction(nameof(ListShifts));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }
   
            return View(shiftView);
        }

        public async Task<IActionResult> ViewShifts(string id)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            return View(await _context.ShiftRuns.Include(x => x.Shift).Where(x => x.ShiftId == num).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditShift(string id)
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

            var user = await _userManager.GetUserAsync(User);

            var shift = await _context.Shifts.Include(x => x.Client).Include(x => x.House).Where(x => x.ShiftId == num && x.OrganizationId == user.OrganizationId).FirstOrDefaultAsync();
            if (shift == null)
            {
                return NotFound();
            }

            ShiftViewModel shiftView = new()
            {
                OrganizationId = shift.OrganizationId,
                ShiftPatternId = shift.ShiftPatternId,
                StartDate = shift.StartDate,
                EndDate = shift.EndDate,
                Perpetual = shift.Perpetual,
                Sunday = shift.Sunday,
                Monday = shift.Monday,
                Tuesday = shift.Tuesday,
                Wednesday = shift.Wednesday,
                Thursday = shift.Thursday,
                Friday = shift.Friday,
                Saturday = shift.Saturday,
                Note = shift.Note,
                ClientId = shift.ClientId,
                HouseId = shift.HouseId,
                ShiftPattern = await _context.ShiftPatterns.FirstOrDefaultAsync(x => x.ShiftPatternId == shift.ShiftPatternId)
            };

            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == user.OrganizationId), "HouseId", "HouseName", shift.HouseId);
            ViewData["ShiftPatternId"] = new SelectList(_context.ShiftPatterns.Where(x => x.OrganizationId == user.OrganizationId), "ShiftPatternId", "PatternName", shift.ShiftPatternId);
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(x => x.OrganizationId == user.OrganizationId), "ClientId", "FullName", shift.ClientId);

            return View(shiftView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Shift(int id)
        {
            var shift = await _context.Shifts.Include(x => x.ShiftPattern).Where(x => x.ShiftId == id).FirstOrDefaultAsync();

            if (shift.ShiftPattern.StartTime <= DateTime.Now)
            {
                ViewBag.Message = "Shift time is past";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);

            int assignedCount = await _context.ShiftAssigments.Where(x => x.ShiftRunId == id && !x.IsDeclined).CountAsync();

            //if (assignedCount >= shift.NumbersRequired)
            //{
            //    ViewBag.Message = "Required number reached";
            //    return View();
            //}

            bool assigned = await _context.ShiftAssigments.Include(x => x.Employee).Where(x => x.ShiftRunId == id && x.Employee.Email.Equals(user.UserName)).AnyAsync();

            if (assigned)
            {
                ViewBag.Message = "Shift already assigned before";
                return View();
            }

            Employee employee = await _context.Employees.Where(x => x.Email.Equals(user.UserName)).FirstOrDefaultAsync();

            var assignedBy = await _userManager.GetUserAsync(User);

            ShiftAssigment assign = new()
            {
                ShiftRunId = id,
                EmployeeId = employee.EmployeeId,
                DateAssigned = DateTime.UtcNow,
                AssignedBy = assignedBy.UserName
            };

            _context.Update(assign);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Shift successfully assigned";
            return View();
        }

        public async Task<IActionResult> AssignShift(string id)
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

            var user = await _userManager.GetUserAsync(User);

            ShiftRun shiftRun = await _context.ShiftRuns.FirstOrDefaultAsync(x => x.ShiftRunId == num);
            Shift shift = await _context.Shifts.Include(x => x.ShiftPattern).Include(x => x.Client).Include(x => x.House).FirstOrDefaultAsync(x => x.ShiftId == shiftRun.ShiftId && x.OrganizationId == user.OrganizationId);
            List<Employee> employees = await _context.Employees.Include(x => x.Department).Include(x => x.JobTitle).Where(x => x.OrganizationId == user.OrganizationId && x.IsActive).ToListAsync();
            List<EmployeeViewModel> employeesViewModelList = new();

            ShiftRunViewModel shiftRunView = new()
            {
                ShiftRunId = shiftRun.ShiftRunId,
                ShiftId = shiftRun.ShiftId,
                ShiftDate = shiftRun.ShiftDate,
                ShiftTime = shiftRun.ShiftTime,
                IsAssigned = shiftRun.IsAssigned,
                NumbersAssigned = shiftRun.NumbersAssigned,
            };

            foreach (var employee in employees)
            {
                EmployeeViewModel employeeView = new()
                {
                    EmployeeId = employee.EmployeeId,
                    OrganizationId = employee.OrganizationId,
                    DepartmentId = employee.DepartmentId,
                    Department = employee.Department,
                    JobTitleId = employee.JobTitleId,
                    JobTitle = employee.JobTitle,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    MiddleName = employee.MiddleName,
                };

                employeesViewModelList.Add(employeeView);
            }

            shiftRunView.Shift = shift;
            shiftRunView.EmployeeViewList = employeesViewModelList;

            return View(shiftRunView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AssignedShift(int id, string assignedUser)
        {
            if (assignedUser == null)
            {
                return Json("No assigned worker");
            }

            var shift = await _context.Shifts.Include(x => x.ShiftPattern).Where(x => x.ShiftId == id).FirstOrDefaultAsync();

            if (shift.ShiftPattern.StartTime <= DateTime.Now)
            {
                return Json("Shift time is past");
            }

            var user = await _userManager.FindByNameAsync(assignedUser);

            //int assignedCount = await _context.ShiftAssigments.Include(x => x.ShiftRun).Where(x => x.ShiftRunId == id && !x.IsDeclined).CountAsync();

            //if (assignedCount >= assignedCount..NumbersRequired)
            //{
            //    return Json("Required number reached");
            //}

            bool assigned = await _context.ShiftAssigments.Include(x => x.Employee).Where(x => x.ShiftRunId == id && x.Employee.Email.Equals(user.UserName)).AnyAsync();

            if (assigned)
            {
                return Json("Shift already assigned before");
            }

            Employee employee = await _context.Employees.Where(x => x.Email.Equals(user.UserName)).FirstOrDefaultAsync();

            var assignedBy = await _userManager.GetUserAsync(User);

            ShiftAssigment assign = new ()
            {
                ShiftRunId = id,
                EmployeeId = employee.EmployeeId,
                DateAssigned = DateTime.UtcNow,
                AssignedBy = assignedBy.UserName             
            };

            _context.Update(assign);
            await _context.SaveChangesAsync();

            return Json("Shift successfully assigned");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShiftDecline(int id)
        {
            var shift = await _context.Shifts.Where(x => x.ShiftId == id).FirstOrDefaultAsync();

            if (shift.StartDate <= DateTime.Now)
            {
                ViewBag.Message = "Shift time is past";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);

            var assigned = await _context.ShiftAssigments.Include(x => x.Employee).Where(x => x.ShiftRunId == id && x.Employee.Email.Equals(user.UserName)).FirstOrDefaultAsync();

            assigned.IsDeclined = true;

            _context.Update(assigned);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Shift successfully de-assigned";
            return View();
        }

        [Route("Schedule/List")]
        public IActionResult Schedule()
        {
            return View();
        }

        [HttpGet]
        [Route("ShiftManagement/GetEvents")]
        // Return all user-saved events
        public async Task<JsonResult> GetShifts()
        {
            var TotalShifts = await _context.Shifts.Include(x => x.ShiftPattern).ToListAsync();

            List<Schedule> schedules = new();

            foreach (var shift in TotalShifts)
            {
                var assignees = await _context.ShiftAssigments.Where(x => x.ShiftRunId == shift.ShiftId).ToListAsync();

                foreach (var assignee in assignees)
                {
                    Schedule schedule = new()
                    {
                        Title = $"{assignee.Employee.FirstName} ({assignee.EmployeeId})",
                        StartTime = (DateTime)shift.ShiftPattern.StartTime,
                        EndTime = (DateTime)shift.ShiftPattern.EndTime,
                        Color = GetRandColor()
                    };

                    schedules.Add(schedule);
                }

            }

            return Json(schedules);
        }

        private static string GetRandColor()
        {
            Random rnd = new();
            string hexOutput = String.Format($"{rnd.Next(0, 0xFFFFFF):X}");
            while (hexOutput.Length < 6)
                hexOutput = "0" + hexOutput;
            return "#" + hexOutput;
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
                _logger.Log(LogLevel.Error, ex, "An error has occurred fetching item");
                return 0;
            }
        }

        public bool ShiftPatternExists(int id)
        {
            return _context.ShiftPatterns.Any(x => x.ShiftPatternId == id);
        }

        private async Task SetShiftRun(Shift shift)
        {
            if (!shift.Perpetual)
            {
                await SetShiftDate(shift, shift.StartDate);
            }
            else
            {
                foreach(DateTime day in EachDay(shift.StartDate, shift.EndDate))
                {
                    if ((day.DayOfWeek == DayOfWeek.Sunday) && shift.Sunday) 
                    {
                        await SetShiftDate(shift, day);
                    }
                    if ((day.DayOfWeek == DayOfWeek.Monday) && shift.Monday)
                    {
                        await SetShiftDate(shift, day);
                    }
                    if ((day.DayOfWeek == DayOfWeek.Tuesday) && shift.Tuesday)
                    {
                        await SetShiftDate(shift, day);
                    }
                    if ((day.DayOfWeek == DayOfWeek.Wednesday) && shift.Wednesday)
                    {
                        await SetShiftDate(shift, day);
                    }
                    if ((day.DayOfWeek == DayOfWeek.Thursday) && shift.Thursday)
                    {
                        await SetShiftDate(shift, day);
                    }
                    if ((day.DayOfWeek == DayOfWeek.Friday) && shift.Friday)
                    {
                        await SetShiftDate(shift, day);
                    }
                    if ((day.DayOfWeek == DayOfWeek.Saturday) && shift.Saturday)
                    {
                        await SetShiftDate(shift, day);
                    }
                }

            }

            return;
        }

        private static IEnumerable<DateTime> EachDay(DateTime fromDate, DateTime? toDate)
        {
            for (var day = fromDate.Date; day.Date <= toDate?.Date; day = day.AddDays(1))
                yield return day;
        }

        private async Task SetShiftDate(Shift shift,  DateTime date)
        {
            ShiftRun shiftRun = new()
            {
                ShiftId = shift.ShiftId,
                ShiftDate = date,
                ShiftTime = shift.ShiftPattern.StartTime
            };

            await _context.AddAsync(shiftRun);
            await _context.SaveChangesAsync();

            return;
        }

        //private bool CheckDuplicateShift(Shift shift)
        //{
        //    return _context.Shifts.Any(x => x.OrganizationId == shift.OrganizationId 
        //                                && x.ShiftPatternId == shift.ShiftPatternId
        //                                && x.S);
        //}
    }
}

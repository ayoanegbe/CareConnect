using CareConnect.Data;
using CareConnect.Interfaces;
using CareConnect.Models;
using CareConnect.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CareConnect.Controllers
{
    public class ShiftManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomIDataProtection _protector;
        private readonly ILogger<EmployeeManagementController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuditTrailService _audit;
        private readonly RecaptchaService _recaptchaService;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;

        public ShiftManagementController(
            Logger<EmployeeManagementController> logger,
            ApplicationDbContext context,
            CustomIDataProtection protector,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuditTrailService audit,
            RecaptchaService recaptchaService,
            IFileService fileService,
            IWebHostEnvironment environment,
            IEmailSender emailSender,
            EmailSettings emailSettings
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
            _emailSettings = emailSettings;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Shifts.ToListAsync());
        }

        public async Task<IActionResult> AddShift()
        {
            var user = await _userManager.GetUserAsync(User);

            Shift shift = new();


            ViewData["ShiftPatternId"] = new SelectList(_context.ShiftPatterns.Where(x => x.OrganizationId == (int)user.OrganizationId), "ShiftPatternId", "PatternName");
            ViewData["ClientId"] = new SelectList(_context.ShiftPatterns.Where(x => x.OrganizationId == (int)user.OrganizationId), "ClientId", "FullName");
            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == (int)user.OrganizationId), "HouseId", "HouseName");

            return View(shift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShift([Bind("ShiftId,ShiftPatternId,StartDate,Perpetual,Note,IsAssigned,NumbersRequired,ClientId,HouseId")] Shift shift)
        {
            var user = await _userManager.GetUserAsync(User);

            ViewData["ShiftPatternId"] = new SelectList(_context.ShiftPatterns.Where(x => x.OrganizationId == (int)user.OrganizationId), "ShiftPatternId", "PatternName", shift.ShiftPatternId);
            ViewData["ClientId"] = new SelectList(_context.ShiftPatterns.Where(x => x.OrganizationId == (int)user.OrganizationId), "ClientId", "FullName", shift.ClientId);
            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == (int)user.OrganizationId), "HouseId", "HouseName", shift.HouseId);

            if (shift.ShiftPatternId == 0)
            {
                ViewBag.Message = "Shift pattern not selected";
                return View(shift);
            }

            if (shift.StartDate < DateTime.Today)
            {
                ViewBag.Message = "Shift date can't be less than today!";
                return View(shift);
            }

            if (shift.StartDate == DateTime.Today && shift.ShiftPattern.StartTime < DateTime.Now)
            {
                ViewBag.Message = "Shift start time can't be less than current time!";
                return View(shift);
            }

            if (string.IsNullOrEmpty(shift.Note))
            {
                ViewBag.Message = "Note field can't be empty!";
                return View(shift);
            }

            if (ModelState.IsValid)
            {
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(shift);
        }

        [HttpGet]
        public async Task<IActionResult> Shift(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts.Include(x => x.Client).Include(x => x.House).Where(x => x.ShiftId == id).FirstOrDefaultAsync();
            if (shift == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            ViewData["user"] = user;

            var users = await _userManager.Users.ToListAsync();

            var usrs = new List<ApplicationUser>();

            foreach (var usr in users)
            {
                if (await _userManager.IsInRoleAsync(usr, "User"))
                    usrs.Add(usr);
            }

            ViewData["users"] = new SelectList(usrs.Where(x => x.OrganizationId == user.OrganizationId), "UserName", "FullName");
            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == user.OrganizationId), "HouseId", "HouseName", shift.HouseId);
            ViewData["ShiftPatternId"] = new SelectList(_context.ShiftPatterns.Where(x => x.OrganizationId == user.OrganizationId), "ShiftPatternId", "PatternName", shift.ShiftPatternId);
            ViewData["ClientId"] = new SelectList(_context.ShiftPatterns.Where(x => x.OrganizationId == user.OrganizationId), "ClientId", "FullName", shift.ClientId);

            return View(shift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Shift(int id)
        {
            var shift = await _context.Shifts.Include(x => x.ShiftPattern).Where(x => x.ShiftId == id).FirstOrDefaultAsync();

            if (shift.ShiftPattern.StartTime <= DateTime.UtcNow)
            {
                ViewBag.Message = "Shift time is past";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);

            int assignedCount = await _context.ShiftAssigments.Where(x => x.ShiftId == id && !x.IsDeclined).CountAsync();

            if (assignedCount >= shift.NumbersRequired)
            {
                ViewBag.Message = "Required number reached";
                return View();
            }

            bool assigned = await _context.ShiftAssigments.Include(x => x.Employee).Where(x => x.ShiftId == id && x.Employee.Email.Equals(user.UserName)).AnyAsync();

            if (assigned)
            {
                ViewBag.Message = "Shift already assigned before";
                return View();
            }

            Employee employee = await _context.Employees.Where(x => x.Email.Equals(user.UserName)).FirstOrDefaultAsync();

            var assignedBy = await _userManager.GetUserAsync(User);


            ShiftAssigment assign = new()
            {
                ShiftId = id,
                EmployeeId = employee.EmployeeId,
                DateAssigned = DateTime.UtcNow,
                AssignedBy = assignedBy.UserName,
                StartDate = shift.StartDate,
            };

            _context.Update(assign);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Shift successfully assigned";
            return View();
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

            if (shift.ShiftPattern.StartTime <= DateTime.UtcNow)
            {
                return Json("Shift time is past");
            }

            var user = await _userManager.FindByNameAsync(assignedUser);

            int assignedCount = await _context.ShiftAssigments.Where(x => x.ShiftId == id && !x.IsDeclined).CountAsync();

            if (assignedCount >= shift.NumbersRequired)
            {
                return Json("Required number reached");
            }

            bool assigned = await _context.ShiftAssigments.Include(x => x.Employee).Where(x => x.ShiftId == id && x.Employee.Email.Equals(user.UserName)).AnyAsync();

            if (assigned)
            {
                return Json("Shift already assigned before");
            }

            Employee employee = await _context.Employees.Where(x => x.Email.Equals(user.UserName)).FirstOrDefaultAsync();

            var assignedBy = await _userManager.GetUserAsync(User);

            ShiftAssigment assign = new ()
            {
                ShiftId = id,
                EmployeeId = employee.EmployeeId,
                DateAssigned = DateTime.UtcNow,
                AssignedBy = assignedBy.UserName,
                StartDate = shift.StartDate,               
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

            if (shift.StartDate <= DateTime.UtcNow)
            {
                ViewBag.Message = "Shift time is past";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);

            var assigned = await _context.ShiftAssigments.Include(x => x.Employee).Where(x => x.ShiftId == id && x.Employee.Email.Equals(user.UserName)).FirstOrDefaultAsync();

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
                var assignees = await _context.ShiftAssigments.Where(x => x.ShiftId == shift.ShiftId).ToListAsync();

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
    }
}

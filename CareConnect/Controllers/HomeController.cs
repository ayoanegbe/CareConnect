using CareConnect.Data;
using CareConnect.Interfaces;
using CareConnect.Models;
using CareConnect.Models.DataViewModels;
using CareConnect.Services;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;

namespace CareConnect.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private const int DATE_DEDUCT = -29;

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;
        private readonly CustomIDataProtection _protector;
        //private readonly CookieDataProtector _cookieProtector;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly RecaptchaService _recaptchaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ITenantContext _tenantContext;

        //private readonly DateTime startDate = DateTime.Today.AddDays(DATE_DEDUCT);
        //private readonly DateTime endDate = DateTime.UtcNow;

        //[TempData]
        //private string anonymousUser { get; set; }

        const int COUNT = 4;

        public HomeController(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            IEmailSender emailSender,
            IOptions<EmailSettings> emailSettings,
            CustomIDataProtection protector,
            //CookieDataProtector cookieProtector,
            ILogger<HomeController> logger,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            RecaptchaService recaptchaService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IStringLocalizer<HomeController> localizer,
            ITenantContext tenantContext
            )
        {
            _context = context;
            _environment = environment;
            _emailSender = emailSender;
            _emailSettings = emailSettings.Value;
            _protector = protector;
            //_cookieProtector = cookieProtector;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _recaptchaService = recaptchaService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _localizer = localizer;
            _tenantContext = tenantContext;
        }

        //[Route("Home/Dashboard")]
        public IActionResult Index()
        {
            //DateRangeViewModel dateRangeView = new ()
            //{
            //    StartDate = startDate,
            //    EndDate = endDate
            //};

            //var shifts = await _context.Shifts.Where(x => x.ShiftDate >= DateTime.Today && x.StartTime >= DateTime.UtcNow).ToListAsync();

            //IList<ApplicationUser> users = await _userManager.GetUsersInRoleAsync("User");

            //StatisticsDataViewModel statisticsData = new()
            //{
            //    TotalShifts = await _context.Shifts.CountAsync(),
            //    TotalAssignedShifts = await _context.ShiftAssigments.Where(x => !x.IsDeclined).CountAsync(),
            //    TodayShifts = await _context.Shifts.Where(x => x.ShiftDate == DateTime.Today).CountAsync(),
            //    TodayAssignedShifts = await _context.Shifts.Where(x => x.ShiftDate == DateTime.Today && x.IsAssigned).CountAsync(),
            //    ActiveUsers = users.Count(),
            //    TotalUnassignedShifts = await _context.ShiftAssigments.Where(x => x.IsDeclined).CountAsync()
            //};

            //ViewData["statisticsData"] = statisticsData;
            //ViewData["shifts"] = shifts;

            //ChartDataListModel chartDataList = new();

            //chartDataList = ChartData();

            //ViewData["chart"] = GetChart(chartDataList.Date, chartDataList.Visits);

            //ViewData["strChart"] = strData();

            //if (!HttpContext.User.Identity.IsAuthenticated)
            //    return View();

            //var user = await _userManager.GetUserAsync(User);

            //var assigned = await _context.ShiftAssigments.Where(x => x.UserName.Equals(user.UserName)).ToListAsync();

            //var userShifts = await _context.Shifts.Include(x => x.ShiftAssigment).ToListAsync();

            //foreach (var shift in userShifts)
            //{
            //    foreach (var assignedShift in assigned)
            //    {

            //    }
            //}

            //ViewData["userShifts"] = userShifts;

            return View();
        }

        

        public Chart GetChart(List<string> labelList, List<double?> dataList)
        {
            Chart chart = new ()
            {
                Type = ChartJSCore.Models.Enums.ChartType.Line
            };

            ChartJSCore.Models.Data data = new()
            {
                Labels = labelList
            };

            LineDataset dataset = new ()
            {
                Label = "Shifts",
                Data = dataList,
                Fill = Convert.ToString(false),
                Tension = 0.1,
                BackgroundColor = new List<ChartColor> { ChartColor.FromRgba(75, 192, 192, 0.4) },
                BorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor> { ChartColor.FromRgba(75, 192, 192, 1) },
                PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor> { ChartColor.FromRgba(75, 192, 192, 1) },
                PointHoverBorderColor = new List<ChartColor> { ChartColor.FromRgba(220, 220, 220, 1) },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            data.Datasets = new List<Dataset>
            {
                dataset
            };

            chart.Data = data;

            return chart;
        }

        //[HttpGet]
        //[Route("Home/ChartData")]
        public ChartDataListModel ChartData()
        {
            var TotalShifts = _context.Shifts.ToList();

            var convertedData = TotalShifts.Select(c => new
            {
                Date = c.ShiftPatternId.ToString("MMM")
            }).ToList();

            var sessionGraphData = convertedData
                .GroupBy(g => g.Date)
                .Select(s => new
                {
                    Date = s.Key,
                    Shift = s.Count()
                }).OrderBy(x => x.Date);

            var Labels = sessionGraphData.Select(s => s.Date);

            List<string> labelList = new();

            foreach (var label in Labels)
            {
                labelList.Add(label);
            }

            var colData = sessionGraphData.Select(s => s.Shift);

            List<double?> dataList = new();

            foreach (var c in colData)
            {
                dataList.Add(Convert.ToInt32(c));
            }

            ChartDataListModel chartDataList = new ()
            {
                Date = labelList,
                Visits = dataList
            };

            return chartDataList;
        }

        //[HttpGet]
        //[Route("Home/strData")]
        public string strData()
        {
            var TotalShifts = _context.Shifts.ToList();

            var convertedData = TotalShifts.Select(c => new
            {
                Date = c.StartDate.ToString("MMM")
            }).ToList();

            var sessionGraphData = convertedData
                .GroupBy(g => g.Date)
                .Select(s => new
                {
                    Date = s.Key,
                    Shift = s.Count()
                }).OrderBy(x => x.Date);

            StringBuilder sb = new ();
            sb.Append("[");

            foreach (var data in sessionGraphData)
            {
                sb.Append("[");
                sb.Append(string.Format($"'{data.Date}', {data.Shift}"));
                sb.Append("],");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return sb.ToString();
        }


        

        

        

        public async Task<IActionResult> ActivityReport(DateRangeViewModel dateRange)
        {
            var shifts = await _context.Shifts.Include(x => x.ShiftPattern).Include(x => x.ShiftAssigment).Where(s => s.ShiftPattern.StartTime >= dateRange.StartDate && s.ShiftPattern.EndTime <= dateRange.EndDate && !s.ShiftAssigment.IsDeclined).ToListAsync();

            foreach (var shift in shifts)
            {
                var house = await _context.Houses.Where(x => x.HouseId == shift.HouseId).FirstOrDefaultAsync();
                shift.House = house;
            }
            //var assignments = await _context.ShiftAssigments.Include(s => s.Shift).ToListAsync();
            return View(shifts);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
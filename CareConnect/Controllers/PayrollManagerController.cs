using CareConnect.Data;
using CareConnect.Models;
using CareConnect.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareConnect.Controllers
{
    public class PayrollManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomIDataProtection _protector;
        private readonly ILogger<PayrollManagerController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public PayrollManagerController(
            ILogger<PayrollManagerController> logger,
            ApplicationDbContext context,
            CustomIDataProtection protector,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _logger = logger;
            _context = context;
            _protector = protector;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

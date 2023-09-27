using CareConnect.Data;
using CareConnect.Enums;
using CareConnect.Interfaces;
using CareConnect.Models;
using CareConnect.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CareConnect.Controllers
{
    public class SetupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomIDataProtection _protector;
        private readonly ILogger<SetupController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuditTrailService _audit;

        public SetupController(
            ILogger<SetupController> logger,
            ApplicationDbContext context,
            CustomIDataProtection protector,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuditTrailService audit
            )
        {

            _logger = logger;
            _context = context;
            _protector = protector;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _audit = audit;
        }

        ////Case Manager
        //public async Task<IActionResult> ListCaseManagers()
        //{
        //    return View(await _context.CaseManagers.Include(m => m.Employee).ToListAsync());
        //}

        //// Case Manager
        //public IActionResult AddCaseManager()
        //{
        //    CaseManager caseManager = new();

        //    ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName");

        //    return View(caseManager);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddCaseManager([Bind("CaseManagerId,OrganizationId,EmployeeId")] CaseManager caseManager)
        //{
        //    var user = await _userManager.GetUserAsync(User);

        //    if (user.OrganizationId != null)
        //    {
        //        caseManager.OrganizationId = (int)user.OrganizationId;
        //    }

        //    caseManager.AddedBy = user.UserName;

        //    if (ModelState.IsValid)
        //    {
        //        await _context.AddAsync(caseManager);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(ListCaseManagers));
        //    }

        //    ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName", caseManager.EmployeeId);

        //    return View(caseManager);
        //}

        //public async Task<IActionResult> EditCaseManager(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    string decodedString = string.Empty;
        //    int decodedNumber = 0;
        //    try
        //    {
        //        decodedString = _protector.Decode(id);
        //        decodedNumber = int.Parse(decodedString);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    var caseManager = await _context.CaseManagers.Include(m => m.Employee).FirstOrDefaultAsync(x => x.CaseManagerId == decodedNumber);
        //    if (caseManager == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName", caseManager.EmployeeId);

        //    return View(caseManager);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditCaseManager(string id, [Bind("CaseManagerId,OrganizationId,EmployeeId")] CaseManager caseManager)
        //{
        //    string decodedString = string.Empty;
        //    int decodedNumber = 0;
        //    try
        //    {
        //        decodedString = _protector.Decode(id);
        //        decodedNumber = int.Parse(decodedString);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    if (decodedNumber != caseManager.CaseManagerId)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _userManager.GetUserAsync(User);
        //    caseManager.UpdatedBy = user.UserName;
        //    caseManager.DateUpdated = DateTime.UtcNow;

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(caseManager);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException ex)
        //        {
        //            if (!CaseManagerExists(caseManager.CaseManagerId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
        //                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //            }
        //        }
        //        return RedirectToAction(nameof(ListCaseManagers));
        //    }

        //    return View(caseManager);
        //}

        // Client
        public async Task<IActionResult> ListClients()
        {
            return View(await _context.Clients.ToListAsync());
        }

        public IActionResult AddClient()
        {
            Client client = new();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClient([Bind("ClientId,CustomerId,OrganizationId,HouseId,ResidentialType,FirstName,MiddieName,LastName,Gender,DateJoined,BirthDate,Phone,Email,EmergencyContactPhone,EmergencyContactAddress,Relationship,GuadianPhoneNumber,FamilyPhysician,Psychiatrist,Budget,CurrencyId,BudgetStartDate,BudgetEndDate,Notes,Comment")] Client client)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                client.OrganizationId = (int)user.OrganizationId;
            }

            client.AddedBy = user.UserName;

            if (client.Notes != null)
            {
                StringBuilder stringBuilder = new();
                stringBuilder = stringBuilder.Append($"[{DateTime.UtcNow}] ==> {user.UserName}");
                stringBuilder = stringBuilder.Append($"<p>{client.Comment}</p>");
                stringBuilder = stringBuilder.Append(new string('-', 50));

                client.Notes = stringBuilder.ToString();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(client);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(client);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, client.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                    return RedirectToAction(nameof(ListClients));
                }
                else
                {
                    ViewBag.Message = "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.";
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,
                            $"An error has occurred when trying to write into {client.GetType().Name} table",
                            ex);
            }
            

            return View(client);
        }

        public async Task<IActionResult> EditClient(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == decodedNumber);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClient(string id, [Bind("ClientId,CustomerId,OrganizationId,HouseId,ResidentialType,FirstName,MiddieName,LastName,Gender,DateJoined,BirthDate,Phone,Email,EmergencyContactPhone,EmergencyContactAddress,Relationship,GuadianPhoneNumber,FamilyPhysician,Psychiatrist,Budget,CurrencyId,BudgetStartDate,BudgetEndDate,Notes,Comment")] Client client)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != client.ClientId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            client.UpdatedBy = user.UserName;
            client.DateUpdated = DateTime.UtcNow;

            if (client.Notes != null)
            {
                StringBuilder stringBuilder = new();
                stringBuilder = stringBuilder.Append(Environment.NewLine);
                stringBuilder = stringBuilder.Append($"[{DateTime.UtcNow}] ==> {user.UserName}");
                stringBuilder = stringBuilder.Append($"<p>{client.Comment}</p>");
                stringBuilder = stringBuilder.Append(new string('-', 50));

                client.Notes += stringBuilder.ToString();
            }

            var oldValue = JsonConvert.SerializeObject(await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == client.ClientId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(client);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, client.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ClientExists(client.ClientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListClients));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(client);
        }



        // Currency
        public async Task<IActionResult> ListCurrencies()
        {
            return View(await _context.Currencies.ToListAsync());
        }

        public IActionResult AddCurrency()
        {
            Currency currency = new();
            return View(currency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCurrency([Bind("CurrencyId,Code,Name,Symbol")] Currency currency)
        {
            var user = await _userManager.GetUserAsync(User);

            currency.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(currency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListCurrencies));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(currency);
        }

        public async Task<IActionResult> EditCurrency(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == decodedNumber);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCurrency(string id, [Bind("CurrencyId,Code,Name,Symbol")] Currency currency)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != currency.CurrencyId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            currency.UpdatedBy = user.UserName;
            currency.DateUpdated = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currency);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!CurrencyExists(currency.CurrencyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListCurrencies));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(currency);
        }

        // Customers
        public async Task<IActionResult> ListCustomers()
        {
            return View(await _context.Customers.ToListAsync());
        }

        public IActionResult AddCustomer()
        {
            Customer customer = new();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomer([Bind("CustomerId,OrganizationId,Name,Address,Phone,Email,ContactPersonName,ContactPersonPhone,Notes,Comment")] Customer customer)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                customer.OrganizationId = (int)user.OrganizationId;
            }

            customer.AddedBy = user.UserName;

            if (customer.Notes != null)
            {
                StringBuilder stringBuilder = new();
                stringBuilder = stringBuilder.Append($"[{DateTime.UtcNow}] ==> {user.UserName}");
                stringBuilder = stringBuilder.Append($"<p>{customer.Comment}</p>");
                stringBuilder = stringBuilder.Append(new string('-', 50));

                customer.Notes = stringBuilder.ToString();
            }

            if (ModelState.IsValid)
            {
                await _context.AddAsync(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListClients));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(customer);
        }

        public async Task<IActionResult> EditCustomer(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == decodedNumber);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(string id, [Bind("CustomerId,OrganizationId,Name,Address,Phone,Email,ContactPersonName,ContactPersonPhone,Notes")] Customer customer)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != customer.CustomerId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            customer.UpdatedBy = user.UserName;
            customer.DateUpdated = DateTime.UtcNow;

            if (customer.Notes != null)
            {
                StringBuilder stringBuilder = new();
                stringBuilder = stringBuilder.Append(Environment.NewLine);
                stringBuilder = stringBuilder.Append($"[{DateTime.UtcNow}] ==> {user.UserName}");
                stringBuilder = stringBuilder.Append($"<p>{customer.Comment}</p>");
                stringBuilder = stringBuilder.Append(new string('-', 50));

                customer.Notes += stringBuilder.ToString();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListCustomers));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(customer);
        }

        public async Task<IActionResult> ListDepartments()
        {
            return View(await _context.Departments.ToListAsync());
        }

        public IActionResult AddDepartment()
        {
            Department department = new();

            return View(department);
        }

        public async Task<IActionResult> AddDepartment([Bind("DepartmentId,Name")] Department department)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                department.OrganizationId = (int)user.OrganizationId;
            }

            department.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListDepartments));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(department);
        }

        public async Task<IActionResult> EditDepartment(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var department = await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == decodedNumber);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(string id, [Bind("DepartmentId,Name")] Department department)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != department.DepartmentId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            department.UpdatedBy = user.UserName;
            department.DateUpdated = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!DepartmentExists(department.DepartmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListDepartments));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(department);
        }

        // Houses
        public async Task<IActionResult> ListHouses()
        {
            return View(await _context.Houses.ToListAsync());
        }

        public IActionResult AddHouse()
        {
            House house = new();

            return View(house);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHouse([Bind("HouseId,HouseName,Organization,CompanyId,Address,City,PostCode,Longitude,Latitude")] House house)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                house.OrganizationId = (int)user.OrganizationId;
            }

            house.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(house);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListClients));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(house);
        }

        public async Task<IActionResult> EditHouse(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var house = await _context.Houses.FirstOrDefaultAsync(x => x.HouseId == decodedNumber);
            if (house == null)
            {
                return NotFound();
            }

            return View(house);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHouse(string id, [Bind("HouseId,HouseName,Organization,CompanyId,Address,City,PostCode,Longitude,Latitude")] House house)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != house.HouseId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            house.UpdatedBy = user.UserName;
            house.DateUpdated = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(house);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!HouseExists(house.HouseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListHouses));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(house);
        }

        public async Task<IActionResult> ListJobTitles()
        {
            return View(await _context.JobTitles.ToListAsync());
        }

        public IActionResult AddJobTile()
        {
            JobTitle jobTitle = new ();

            return View(jobTitle);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddJobTitle([Bind("JobTitleId,Title,Description")] JobTitle jobTitle)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                jobTitle.OrganizationId = (int)user.OrganizationId;
            }

            jobTitle.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(jobTitle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListJobTitles));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(jobTitle);
        }

        public async Task<IActionResult> EditJobTile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var jobTitle = await _context.JobTitles.FirstOrDefaultAsync(x => x.JobTitleId == decodedNumber);
            if (jobTitle == null)
            {
                return NotFound();
            }

            return View(jobTitle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditJobTile(string id, [Bind("JobTitleId,Title,Description")] JobTitle jobTitle)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != jobTitle.JobTitleId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            jobTitle.UpdatedBy = user.UserName;
            jobTitle.DateUpdated = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobTitle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!JobTitleExists(jobTitle.JobTitleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListJobTitles));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(jobTitle);
        }

        // Organization
        public async Task<IActionResult> ListOrganizations()
        {
            return View(await _context.Organizations.Include(t => t.Tenant).ToListAsync());
        }

        public IActionResult AddOrganization()
        {
            Organization organization = new ();

            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Name");

            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrganization([Bind("OrganizationId,TenantId,Name,Address,Email,Phone,Website")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListOrganizations));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Name", organization.TenantId);

            return View(organization);
        }

        public async Task<IActionResult> EditOrganization(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var organization = await _context.Organizations.Include(t => t.Tenant).FirstOrDefaultAsync(x => x.OrganizationId == decodedNumber);
            if (organization == null)
            {
                return NotFound();
            }

            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Name", organization.TenantId);

            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrganization(string id, [Bind("OrganizationId,TenantId,Name,Address,Email,Phone,Website")] Organization organization)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != organization.OrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!OrganizationExists(organization.OrganizationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListOrganizations));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Name", organization.TenantId);

            return View(organization);
        }

        public async Task<IActionResult> ListPayGrades()
        {
            return View(await _context.PayGrades.ToListAsync());
        }

        public IActionResult AddPayGrade()
        {
            PayGrade payGrade = new ();

            return View(payGrade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayGrade([Bind("PayGradeId,OrganizationId,Name,Description")] PayGrade payGrade)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                payGrade.OrganizationId = (int)user.OrganizationId;
            }

            payGrade.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(payGrade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListPayGrades));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(payGrade);
        }

        public async Task<IActionResult> EditPayGrade(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var payGrade = await _context.PayGrades.FirstOrDefaultAsync(x => x.PayGradeId == decodedNumber);
            if (payGrade == null)
            {
                return NotFound();
            }

            return View(payGrade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPayGrade(string id, [Bind("PayGradeId,OrganizationId,Name,Description")] PayGrade payGrade)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != payGrade.PayGradeId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            payGrade.UpdatedBy = user.UserName;
            payGrade.DateUpdated = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payGrade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PayGradeExists(payGrade.PayGradeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListPayGrades));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(payGrade);
        }

        public async Task<IActionResult> ListPayGradeLevels()
        {
            return View(await _context.PayGradeLevels.Include(x => x.PayGrade).Include(c => c.Currency).ToListAsync());
        }

        public IActionResult AddPayGradeLevel()
        {
            PayGradeLevel gradeLevel = new();

            ViewData["PayGradeId"] = new SelectList(_context.PayGrades, "PayGradeId", "Name");
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code");

            return View(gradeLevel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayGradeLevel([Bind("PayGradeLevelId,PayGradeId,Description,PayLevel,BasicSalary,HourlyRate,OvertimeRate,CurrencyId")] PayGradeLevel gradeLevel)
        {
            var user = await _userManager.GetUserAsync(User);

            gradeLevel.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(gradeLevel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListPayGradeLevels));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["PayGradeId"] = new SelectList(_context.PayGrades, "PayGradeId", "Name", gradeLevel.PayGradeId);
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code", gradeLevel.CurrencyId);

            return View(gradeLevel);
        }

        public async Task<IActionResult> EditPayGradeLevel(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var gradeLevel = await _context.PayGradeLevels.Include(g => g.PayGrade).Include(s => s.Currency).FirstOrDefaultAsync(x => x.PayGradeLevelId == decodedNumber);
            if (gradeLevel == null)
            {
                return NotFound();
            }

            ViewData["PayGradeId"] = new SelectList(_context.PayGrades, "PayGradeId", "Name", gradeLevel.PayGradeId);
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code", gradeLevel.CurrencyId);

            return View(gradeLevel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPayGradeLevel(string id, [Bind("PayGradeLevelId,PayGradeId,Description,PayLevel,BasicSalary,HourlyRate,OvertimeRate,CurrencyId")] PayGradeLevel gradeLevel)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != gradeLevel.PayGradeLevelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradeLevel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PayGradeLevelExists(gradeLevel.PayGradeLevelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListPayGradeLevels));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["PayGradeId"] = new SelectList(_context.PayGrades, "PayGradeId", "Name", gradeLevel.PayGradeId);
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code", gradeLevel.CurrencyId);

            return View(gradeLevel);
        }

        public async Task<IActionResult> ListPayGradeLevelDeductions()
        {
            return View(await _context.PayGradeLevelDeductions.Include(x => x.PayGradeLevel).ToListAsync());
        }

        public IActionResult AddPayGradeLevelDeduction()
        {
            PayGradeLevelDeduction payGradeLevelDeduction = new ();

            ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels, "PayGradeLevelId", "Description");

            return View(payGradeLevelDeduction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayGradeLevelDeduction([Bind("PayGradeLevelDeductionId,PayGradeLevelId,TaxRate,HealthInsurance,RetirementContribution")] PayGradeLevelDeduction gradeLevelDeduction)
        {
            var user = await _userManager.GetUserAsync(User);

            gradeLevelDeduction.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(gradeLevelDeduction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListPayGradeLevelDeductions));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels, "PayGradeLevelId", "Description", gradeLevelDeduction.PayGradeLevelId);

            return View(gradeLevelDeduction);
        }

        public async Task<IActionResult> EditPayGradeLevelDeduction(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var gradeLevelDeduction = await _context.PayGradeLevelDeductions.Include(g => g.PayGradeLevel).FirstOrDefaultAsync(x => x.PayGradeLevelDeductionId == decodedNumber);
            if (gradeLevelDeduction == null)
            {
                return NotFound();
            }

            ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels, "PayGradeLevelId", "Description", gradeLevelDeduction.PayGradeLevelId);

            return View(gradeLevelDeduction);
        }

        public async Task<IActionResult> EditPayGradeLevelDeduction(string id, [Bind("PayGradeLevelDeductionId,PayGradeLevelId,TaxRate,HealthInsurance,RetirementContribution")] PayGradeLevelDeduction gradeLevelDeduction)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != gradeLevelDeduction.PayGradeLevelDeductionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradeLevelDeduction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PayGradeLevelDeductionExists(gradeLevelDeduction.PayGradeLevelDeductionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListPayGradeLevelDeductions));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels, "PayGradeLevelId", "Description", gradeLevelDeduction.PayGradeLevelId);

            return View(gradeLevelDeduction);
        }

        public async Task<IActionResult> ListSubscriptions()
        {
            return View(await _context.Subscriptions.Include(g => g.Organization).Include(s => s.SubscriptionRate).ToListAsync());
        }

        public IActionResult AddSubscription()
        {
            Subscription subscription = new();

            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name");
            ViewData["SubscriptionRateId"] = new SelectList(_context.SubscriptionsRates, "SubscriptionRateId", "Type");

            return View(subscription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubscription([Bind("SubscriptionId,OrganizationId,SubscriptionRateId,StartDate,EndDate,Status")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListSubscriptionRate));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", subscription.OrganizationId);
            ViewData["SubscriptionRateId"] = new SelectList(_context.SubscriptionsRates, "SubscriptionRateId", "Type", subscription.SubscriptionRateId);

            return View(subscription);
        }

        public async Task<IActionResult> EditSubscription(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var subscription = await _context.Subscriptions.Include(g => g.Organization).Include(s => s.SubscriptionRate).FirstOrDefaultAsync(x => x.SubscriptionRateId == decodedNumber);
            if (subscription == null)
            {
                return NotFound();
            }

            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", subscription.OrganizationId);
            ViewData["SubscriptionRateId"] = new SelectList(_context.SubscriptionsRates, "SubscriptionRateId", "Type", subscription.SubscriptionRateId);

            return View(subscription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubscription(string id, [Bind("SubscriptionId,OrganizationId,SubscriptionRateId,StartDate,EndDate,Status")] Subscription subscription)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != subscription.SubscriptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!SubscriptionExists(subscription.SubscriptionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListSubscriptionRate));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name", subscription.OrganizationId);
            ViewData["SubscriptionRateId"] = new SelectList(_context.SubscriptionsRates, "SubscriptionRateId", "Type", subscription.SubscriptionRateId);

            return View(subscription);
        }

        public async Task<IActionResult> ListSubscriptionRate()
        {
            return View(await _context.SubscriptionsRates.Include(c => c.Currency).ToListAsync());
        }

        public IActionResult AddSubscriptionRate()
        {
            SubscriptionRate subscriptionRate = new ();

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code");

            return View(subscriptionRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubscriptionRate([Bind("SubscriptionRateId,Type,Period,Amount,CurrencyId,NumberOfEmployees,NumberOfClients")] SubscriptionRate subscriptionRate)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(subscriptionRate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListSubscriptionRate));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code", subscriptionRate.CurrencyId);

            return View(subscriptionRate);
        }

        public async Task<IActionResult> EditSubscriptionRate(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var subscriptionRate = await _context.SubscriptionsRates.Include(s => s.Currency).FirstOrDefaultAsync(x => x.SubscriptionRateId == decodedNumber);
            if (subscriptionRate == null)
            {
                return NotFound();
            }

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code", subscriptionRate.CurrencyId);

            return View(subscriptionRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubscriptionRate(string id, [Bind("SubscriptionRateId,Type,Period,Amount,CurrencyId,NumberOfEmployees,NumberOfClients")] SubscriptionRate subscriptionRate)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != subscriptionRate.SubscriptionRateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscriptionRate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!SubscriptionRateExists(subscriptionRate.SubscriptionRateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListSubscriptionRate));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code", subscriptionRate.CurrencyId);

            return View(subscriptionRate);
        }

        public async Task<IActionResult> ListTenants()
        {
            return View(await _context.Tenants.ToListAsync());
        }

        public IActionResult AddTenant()
        {
            Tenant tenant = new();

            return View(tenant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTenant([Bind("TenantId,Name")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(tenant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListTenants));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(tenant);
        }

        public async Task<IActionResult> EditTenant(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.TenantId == decodedNumber);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTenant(string id, [Bind("TenantId,Name")] Tenant tenant)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != tenant.TenantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tenant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TenantExists(tenant.TenantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListTenants));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(tenant);
        }

        public async Task<IActionResult> ListVendors()
        {
            return View(await _context.Vendors.ToListAsync());
        }

        public IActionResult AddVendor()
        {
            Vendor vendor = new();
            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVendor([Bind("VendorId,OrganizationId,Name,Email,Phone,Address,City,PostCode,ContactPerson,PaymentMethod,BankName,BankCode,TransitCode,AccountNumber")] Vendor vendor)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                vendor.OrganizationId = (int)user.OrganizationId;
            }

            vendor.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(vendor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListVendors));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(vendor);
        }

        public async Task<IActionResult> EditVendor(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.VendorId == decodedNumber);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVendor(string id, [Bind("VendorId,OrganizationId,Name,Email,Phone,Address,City,PostCode,ContactPerson,PaymentMethod,BankName,BankCode,TransitCode,AccountNumber")] Vendor vendor)
        {
            string decodedString = string.Empty;
            int decodedNumber = 0;
            try
            {
                decodedString = _protector.Decode(id);
                decodedNumber = int.Parse(decodedString);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (decodedNumber != vendor.VendorId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            vendor.UpdatedBy = user.UserName;
            vendor.DateUpdated = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!VendorExits(vendor.VendorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(ListVendors));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(vendor);
        }

        public async Task<IActionResult> SmtpSettings(int id)
        {
            ViewData["Title"] = "SMTP Settings";
            ViewData["Page"] = "SMTP Settings";

            var smtpSettings = await _context.SmtpSettings.Where(x => x.OrganizationId == id).FirstOrDefaultAsync();

            //PasswordCache = _protector.Decode(smtpSettings.Password);

            return View(smtpSettings);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SmtpSettings([Bind("SmtpSettingId,UserName,Host,HostIP,PortNumber,Password")] SmtpSetting smtp)
        {
            ViewData["Title"] = "SMTP Settings";
            ViewData["Page"] = "SMTP Settings";

            if (string.IsNullOrEmpty(smtp.UserName))
            {
                ViewBag.Message = "User Name is a required field";
                return View(smtp);
            }

            if (string.IsNullOrEmpty(smtp.Host))
            {
                ViewBag.Message = "Host is a required field";
                return View(smtp);
            }

            if (string.IsNullOrEmpty(smtp.HostIP))
            {
                ViewBag.Message = "Host IP is a required field";
                return View(smtp);
            }

            if (string.IsNullOrEmpty(smtp.Password))
            {
                ViewBag.Message = "Password is a required field";
                return View(smtp);
            }

            if (string.IsNullOrEmpty(smtp.PortNumber))
            {
                ViewBag.Message = "Port Number is a required field";
                return View(smtp);
            }

            var user = await _userManager.GetUserAsync(User);            

            var smtpSettings = await _context.SmtpSettings.FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                if (smtpSettings == null)
                {
                    try
                    {
                        //smtp.Password = _protector.Encode(smtp.Password);

                        if (user.OrganizationId != null)
                        {
                            smtp.OrganizationId = (int)user.OrganizationId;
                        }

                        smtp.AddedBy = user.UserName;

                        _context.Add(smtp);
                        await _context.SaveChangesAsync();
                        ViewBag.Success = "SMTP Settings added successfully";
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        ViewBag.Message = $"Database related error -> Error:{ex}";
                    }
                }
                else
                {
                    try
                    {
                        smtp.UpdatedBy = user.UserName;
                        smtp.DateUpdated = DateTime.UtcNow;

                        _context.Update(smtp);
                        await _context.SaveChangesAsync();
                        ViewBag.Success = "SMTP Settings updated successfully";
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        ViewBag.Message = $"Database related error -> Error:{ex}";
                    }
                }
            }

            return View(smtp);
        }

        private bool CaseManagerExists(int id)
        {
            return _context.CaseManagers.Any(c => c.CaseManagerId == id);
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(c => c.ClientId == id);
        }

        private bool CurrencyExists(int id)
        {
            return _context.Currencies.Any(c => c.CurrencyId == id);
        }

        private bool CustomerExists(int id)
        {
            return _context.Currencies.Any(c => c.CurrencyId == id);
        }

        public bool DepartmentExists(int id)
        {
            return _context.Departments.Any(d => d.DepartmentId == id);
        }

        private bool HouseExists(int id)
        {
            return _context.Houses.Any(h => h.HouseId == id);
        }

        private bool JobTitleExists(int id)
        {
            return _context.JobTitles.Any(j => j.JobTitleId == id);
        }
        private bool OrganizationExists(int id)
        {
            return _context.Organizations.Any(c => c.OrganizationId == id);
        }

        private bool PayGradeExists(int id)
        {
            return _context.PayGrades.Any(p => p.PayGradeId == id);
        }

        private bool PayGradeLevelExists(int id)
        {
            return _context.PayGradeLevels.Any(g => g.PayGradeLevelId == id);
        }

        private bool PayGradeLevelDeductionExists(int id)
        {
            return _context.PayGradeLevelDeductions.Any(d => d.PayGradeLevelDeductionId == id);
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscriptions.Any(c => c.SubscriptionId == id);
        }

        private bool SubscriptionRateExists(int id)
        {
            return _context.SubscriptionsRates.Any(s => s.SubscriptionRateId == id);
        }

        private bool TenantExists(int id)
        {
            return _context.Tenants.Any(t => t.TenantId == id);
        }

        private bool VendorExits(int id) 
        {
            return _context.Vendors.Any(v => v.VendorId == id);
        }
    }
}

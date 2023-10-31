using CareConnect.Data;
using CareConnect.Enums;
using CareConnect.Interfaces;
using CareConnect.Models;
using CareConnect.Models.CareConnectViewModels;
using CareConnect.Models.DataViewModels;
using CareConnect.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;

namespace CareConnect.Controllers
{
    public class SetupController : Controller
    {
        private const int DATE_ADD = 1;

        private readonly DateTime startDate = DateTime.Now;
        private readonly DateTime endDate = DateTime.Today.AddMonths(DATE_ADD);

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

        // Client
        public async Task<IActionResult> ListClients()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.Clients.Include(x => x.Customer).Where(x => x.OrganizationId == user.OrganizationId).ToListAsync());
        }

        public async Task<IActionResult> AddClient()
        {
            var user = await _userManager.GetUserAsync(User);

            ClientViewModel client = new()
            {
                BirthDate = DateTime.Now.AddYears(-10),
                DateJoined = DateTime.Now,
                BudgetStartDate = DateTime.Now,
                BudgetEndDate = DateTime.Now.AddYears(1),
            };

            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(x => x.OrganizationId == user.OrganizationId), "CustomerId", "Name");
            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == user.OrganizationId), "HouseId", "HouseName");
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Name");

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClient([Bind("ClientId,CustomerId,OrganizationId,HouseId,ResidentialType,FirstName,MiddleName,LastName,Gender,DateJoined,BirthDate,Phone,Email,EmergencyContactPhone,EmergencyContactAddress,Relationship,GuadianPhoneNumber,FamilyPhysician,Psychiatrist,Budget,CurrencyId,BudgetStartDate,BudgetEndDate,Notes")] ClientViewModel clientView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                clientView.OrganizationId = (int)user.OrganizationId;
            }

            if (clientView.Notes != null)
            {
                StringBuilder stringBuilder = new();
                stringBuilder = stringBuilder.AppendLine($"[{DateTime.Now}] ==> {user.UserAlias}");
                stringBuilder = stringBuilder.Append($"{clientView.Notes}");
                stringBuilder = stringBuilder.AppendLine();
                stringBuilder = stringBuilder.Append(new string('-', 50));

                clientView.Notes = stringBuilder.ToString();
            }

            Client client = new()
            {
                CustomerId = clientView.CustomerId,
                OrganizationId = clientView.OrganizationId,
                HouseId = clientView.HouseId,
                ResidentialType = clientView.ResidentialType,
                FirstName = clientView.FirstName,
                MiddleName = clientView.MiddleName,
                LastName = clientView.LastName,
                Gender = clientView.Gender,
                DateJoined = clientView.DateJoined,
                BirthDate = clientView.BirthDate,
                Phone = clientView.Phone,
                Email = clientView.Email,
                EmergencyContactAddress = clientView.EmergencyContactAddress,
                EmergencyContactPhone = clientView.EmergencyContactPhone,
                Relationship = clientView.Relationship,
                GuadianPhoneNumber = clientView.GuadianPhoneNumber,
                FamilyPhysician = clientView.FamilyPhysician,
                Psychiatrist = clientView.Psychiatrist,
                Budget = clientView.Budget,
                CurrencyId = clientView.CurrencyId,
                BudgetStartDate = clientView.BudgetStartDate,
                BudgetEndDate = clientView.BudgetEndDate,
                Notes = clientView.Notes,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

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

            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(x => x.OrganizationId == user.OrganizationId), "CustomerId", "Name", client.CustomerId);
            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == user.OrganizationId), "HouseId", "HouseName", client.HouseId);
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Name", client.CurrencyId);

            return View(clientView);
        }

        public async Task<IActionResult> EditClient(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == num);
            if (client == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(x => x.OrganizationId == user.OrganizationId), "CustomerId", "Name", client.CustomerId);
            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == user.OrganizationId), "HouseId", "HouseName", client.HouseId);
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Name", client.CurrencyId);

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClient(string id, [Bind("ClientId,CustomerId,OrganizationId,HouseId,ResidentialType,FirstName,MiddleName,LastName,Gender,DateJoined,BirthDate,Phone,Email,EmergencyContactPhone,EmergencyContactAddress,Relationship,GuadianPhoneNumber,FamilyPhysician,Psychiatrist,Budget,CurrencyId,BudgetStartDate,BudgetEndDate,Notes,Comment")] ClientViewModel clientView)
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

            if (num != clientView.ClientId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            Client client = new()
            {
                ClientId = clientView.ClientId,
                CustomerId = clientView.CustomerId,
                OrganizationId = clientView.OrganizationId,
                HouseId = clientView.HouseId,
                ResidentialType = clientView.ResidentialType,
                FirstName = clientView.FirstName,
                MiddleName = clientView.MiddleName,
                LastName = clientView.LastName,
                Gender = clientView.Gender,
                DateJoined = clientView.DateJoined,
                BirthDate = clientView.BirthDate,
                Phone = clientView.Phone,
                Email = clientView.Email,
                EmergencyContactAddress = clientView.EmergencyContactAddress,
                EmergencyContactPhone = clientView.EmergencyContactPhone,
                Relationship = clientView.Relationship,
                GuadianPhoneNumber = clientView.GuadianPhoneNumber,
                FamilyPhysician = clientView.FamilyPhysician,
                Psychiatrist = clientView.Psychiatrist,
                Budget = clientView.Budget,
                CurrencyId = clientView.CurrencyId,
                BudgetStartDate = clientView.BudgetStartDate,
                BudgetEndDate = clientView.BudgetEndDate,
                Notes = clientView.Notes,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientView.ClientId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ChangeTracker.Clear();
                    _context.Update(client);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(client);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, client.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);

                    return RedirectToAction(nameof(ListClients));
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
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(x => x.OrganizationId == user.OrganizationId), "CustomerId", "Name", client.CustomerId);
            ViewData["HouseId"] = new SelectList(_context.Houses.Where(x => x.OrganizationId == user.OrganizationId), "HouseId", "HouseName", client.HouseId);
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Name", client.CurrencyId);

            return View(clientView);
        }

        // Currency
        public async Task<IActionResult> ListCurrencies()
        {
            return View(await _context.Currencies.ToListAsync());
        }

        public IActionResult AddCurrency()
        {
            CurrencyViewModel currency = new();
            return View(currency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCurrency([Bind("CurrencyId,Code,Name,Symbols")] CurrencyViewModel currencyView)
        {
            var user = await _userManager.GetUserAsync(User);

            Currency currency = new()
            {
                CurrencyId = currencyView.CurrencyId,
                Code = currencyView.Code,
                Name = currencyView.Name,
                Symbols = currencyView.Symbols,
                AddedBy = user.UserName
            };

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

            return View(currencyView);
        }

        public async Task<IActionResult> EditCurrency(string id)
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

            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == num);
            if (currency == null)
            {
                return NotFound();
            }

            CurrencyViewModel currencyView = new()
            {
                CurrencyId = currency.CurrencyId,
                Code = currency.Code,
                Name = currency.Name,
                Symbols = currency.Symbols,
            };

            return View(currencyView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCurrency(string id, [Bind("CurrencyId,Code,Name,Symbols")] CurrencyViewModel currencyView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != currencyView.CurrencyId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            Currency currency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == num);

            currency.Code = currencyView.Code;
            currency.Name = currencyView.Name;
            currency.Symbols = currencyView.Symbols;
            currency.UpdatedBy = user.UserName;
            currency.DateUpdated = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currency);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListCurrencies));
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

                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(currencyView);
        }

        // Customers
        public async Task<IActionResult> ListCustomers()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.Customers.Where(x => x.OrganizationId == user.OrganizationId).ToListAsync());
        }

        public IActionResult AddCustomer()
        {
            CustomerViewModel customer = new();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomer([Bind("CustomerId,OrganizationId,Name,CustomerType,Address,Phone,Email,ContactPersonName,ContactPersonPhone,Notes,Comment")] CustomerViewModel customerView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                customerView.OrganizationId = (int)user.OrganizationId;
            }            

            if (customerView.Notes != null)
            {
                StringBuilder stringBuilder = new();
                stringBuilder = stringBuilder.AppendLine($"[{DateTime.Now}] ==> {user.UserAlias}");
                stringBuilder = stringBuilder.Append($"{customerView.Notes}");
                stringBuilder = stringBuilder.AppendLine();
                stringBuilder = stringBuilder.Append(new string('-', 50));

                customerView.Notes = stringBuilder.ToString();
            }

            Customer customer = new()
            {
                OrganizationId = customerView.OrganizationId,
                Name = customerView.Name,
                CustomerType = customerView.CustomerType,
                Address = customerView.Address,
                Phone = customerView.Phone,
                Email = customerView.Email,
                ContactPersonName = customerView.ContactPersonName,
                ContactPersonPhone = customerView.ContactPersonPhone,
                Notes = customerView.Notes,
                AddedBy = user.UserName
            };

            if (ModelState.IsValid)
            {
                await _context.AddAsync(customer);
                await _context.SaveChangesAsync();

                var newValue = JsonConvert.SerializeObject(customer);

                await _audit.UpdateAuditTrail((int)user.OrganizationId, customer.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                return RedirectToAction(nameof(ListCustomers));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(customerView);
        }

        public async Task<IActionResult> EditCustomer(string id)
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

            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == num);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerView = new()
            {
                CustomerId = customer.CustomerId,
                OrganizationId = customer.OrganizationId,
                CustomerType = customer.CustomerType,
                Name = customer.Name,
                Address = customer.Address,
                Phone = customer.Phone,
                Email = customer.Email,
                ContactPersonName = customer.ContactPersonName,
                ContactPersonPhone = customer.ContactPersonPhone,
                Notes = customer.Notes,
                Comment = customer.Comment
            };

            return View(customerView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(string id, [Bind("CustomerId,OrganizationId,Name,CustomerType,Address,Phone,Email,ContactPersonName,ContactPersonPhone,Notes")] CustomerViewModel customerView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != customerView.CustomerId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            Customer customer = new()
            {
                CustomerId = customerView.CustomerId,
                OrganizationId = customerView.OrganizationId,
                Name = customerView.Name,
                CustomerType = customerView.CustomerType,
                Address = customerView.Address,
                Phone = customerView.Phone,
                Email = customerView.Email,
                ContactPersonName = customerView.ContactPersonName,
                ContactPersonPhone = customerView.ContactPersonPhone,
                Notes = customerView.Notes,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == customerView.CustomerId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ChangeTracker.Clear();

                    _context.Update(customer);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(customer);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, customer.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);

                    return RedirectToAction(nameof(ListCustomers));
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
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(customerView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote(NoteViewModel noteView)
        {
            string decodedNum = JsonConvert.DeserializeObject<string>(Utils.DecryptStringAES(noteView.SourceId));
            int num = int.Parse(decodedNum);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var user = await _userManager.GetUserAsync(User);

            string decodedComment = JsonConvert.DeserializeObject<string>(Utils.DecryptStringAES(noteView.Comment));

            StringBuilder stringBuilder = new();
            stringBuilder = stringBuilder.Append(Environment.NewLine);
            stringBuilder = stringBuilder.AppendLine($"[{DateTime.Now}] ==> {user.UserAlias}");
            stringBuilder = stringBuilder.Append($"{decodedComment}");
            stringBuilder = stringBuilder.AppendLine();
            stringBuilder = stringBuilder.Append(new string('-', 50));

            if (noteView.Source == NoteSource.Client)
            {
                Client client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == num);
                client.Notes += stringBuilder.ToString();

                _context.Update(client);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(EditClient), new { id = _protector.Encode(num.ToString()) });
                
            }
            else
            {
                Customer customer = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == num);
                customer.Notes += stringBuilder.ToString();

                _context.Update(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(EditCustomer), new { id = _protector.Encode(num.ToString()) });
            }
            
        }

        public async Task<IActionResult> ListDepartments()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.Departments.Where(x => x.OrganizationId == user.OrganizationId).ToListAsync());
        }

        public IActionResult AddDepartment()
        {
            DepartmentViewModel department = new();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment([Bind("DepartmentId,Name")] DepartmentViewModel departmentView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                departmentView.OrganizationId = (int)user.OrganizationId;
            }

            Department department = new()
            {
                Name = departmentView.Name,
                OrganizationId = departmentView.OrganizationId,
                AddedBy = user.UserName
            };

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

            return View(departmentView);
        }

        public async Task<IActionResult> EditDepartment(string id)
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

            var department = await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == num);
            if (department == null)
            {
                return NotFound();
            }

            DepartmentViewModel departmentView = new()
            {
                DepartmentId = department.DepartmentId,
                OrganizationId = department.OrganizationId,
                Name = department.Name,
            };

            return View(departmentView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(string id, [Bind("DepartmentId,OrganizationId,Name")] DepartmentViewModel departmentView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != departmentView.DepartmentId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            Department department = new()
            {
                DepartmentId = departmentView.DepartmentId,
                OrganizationId = departmentView.OrganizationId,
                Name = departmentView.Name,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListDepartments));
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
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(departmentView);
        }

        // Houses
        public async Task<IActionResult> ListHouses()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.Houses.Where(x => x.OrganizationId == user.OrganizationId).ToListAsync());
        }

        public IActionResult AddHouse()
        {
            HouseViewModel house = new();

            return View(house);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHouse([Bind("HouseId,HouseName,OrganizationId,Address,City,PostCode,Longitude,Latitude")] HouseViewModel houseView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                houseView.OrganizationId = (int)user.OrganizationId;
            }

            House house = new()
            {
                HouseName = houseView.HouseName,
                OrganizationId = houseView.OrganizationId,
                Address = houseView.Address,
                City = houseView.City,
                PostCode = houseView.PostCode,
                Longitude = houseView.Longitude,
                Latitude = houseView.Latitude,
                AddedBy = user.UserName
            };

            if (ModelState.IsValid)
            {
                await _context.AddAsync(house);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListHouses));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(houseView);
        }

        public async Task<IActionResult> EditHouse(string id)
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

            var house = await _context.Houses.FirstOrDefaultAsync(x => x.HouseId == num);
            if (house == null)
            {
                return NotFound();
            }

            HouseViewModel houseView = new()
            {
                HouseId = house.HouseId,
                HouseName = house.HouseName,
                OrganizationId = house.OrganizationId,
                Address = house.Address,
                City = house.City,
                PostCode = house.PostCode,
                Longitude = house.Longitude,
                Latitude = house.Latitude,
            };

            return View(houseView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHouse(string id, [Bind("HouseId,HouseName,OrganizationId,CompanyId,Address,City,PostCode,Longitude,Latitude")] HouseViewModel houseView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != houseView.HouseId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            House house = new()
            {
                HouseId = houseView.HouseId,
                HouseName = houseView.HouseName,
                OrganizationId = houseView.OrganizationId,
                Address = houseView.Address,
                City = houseView.City,
                PostCode = houseView.PostCode,
                Longitude = houseView.Longitude,
                Latitude = houseView.Latitude,
                IsActive = houseView.IsActive,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now,
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(house);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListHouses));
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

                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(houseView);
        }

        public async Task<IActionResult> ListJobTitles()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.JobTitles.Where(x => x.OrganizationId == user.OrganizationId).ToListAsync());
        }

        public IActionResult AddJobTitle()
        {
            JobTitleViewModel jobTitle = new ();

            return View(jobTitle);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddJobTitle([Bind("JobTitleId,Title,Description")] JobTitleViewModel jobTitleView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                jobTitleView.OrganizationId = (int)user.OrganizationId;
            }

            JobTitle jobTitle = new()
            {
                OrganizationId = jobTitleView.OrganizationId,
                Title = jobTitleView.Title,
                Description = jobTitleView.Description,
                AddedBy = user.UserName
            };

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

            return View(jobTitleView);
        }

        public async Task<IActionResult> EditJobTitle(string id)
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

            var jobTitle = await _context.JobTitles.FirstOrDefaultAsync(x => x.JobTitleId == num);
            if (jobTitle == null)
            {
                return NotFound();
            }

            JobTitleViewModel jobTitleView = new()
            {
                JobTitleId = jobTitle.JobTitleId,
                OrganizationId = jobTitle.OrganizationId,
                Title = jobTitle.Title,
                Description = jobTitle.Description
            };

            return View(jobTitleView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditJobTitle(string id, [Bind("JobTitleId,OrganizationId,Title,Description")] JobTitleViewModel jobTitleView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != jobTitleView.JobTitleId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            JobTitle jobTitle = new()
            {
                JobTitleId = jobTitleView.JobTitleId,
                OrganizationId = jobTitleView.OrganizationId,
                Title = jobTitleView.Title,
                Description = jobTitleView.Description,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobTitle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListJobTitles));
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
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(jobTitleView);
        }

        // Organization
        public async Task<IActionResult> ListOrganizations()
        {
            return View(await _context.Organizations.Include(t => t.Tenant).ToListAsync());
        }

        public IActionResult AddOrganization()
        {
            OrganizationViewModel organization = new ();

            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Name");

            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrganization([Bind("OrganizationId,TenantId,Name,Address,Email,Phone,Website")] OrganizationViewModel organizationView)
        {
            var user = await _userManager.GetUserAsync(User);

            Organization organization = new()
            {
                OrganizationId = organizationView.OrganizationId,
                TenantId = organizationView.TenantId,
                Name = organizationView.Name,
                Address = organizationView.Address,
                Email = organizationView.Email,
                Phone = organizationView.Phone,
                Website = organizationView.Website,
                AddedBy = user.UserName
            };

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

            return View(organizationView);
        }

        public async Task<IActionResult> EditOrganization(string id)
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

            var organization = await _context.Organizations.Include(t => t.Tenant).FirstOrDefaultAsync(x => x.OrganizationId == num);
            if (organization == null)
            {
                return NotFound();
            }

            OrganizationViewModel organizationView = new()
            {
                OrganizationId = num,
                TenantId = organization.TenantId,
                Name = organization.Name,
                Address = organization.Address,
                Email = organization.Email,
                Phone = organization.Phone,
                Website = organization.Website,
                IsActive = organization.IsActive,
            };

            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Name", organization.TenantId);

            return View(organizationView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrganization(string id, [Bind("OrganizationId,TenantId,Name,Address,Email,Phone,Website,IsActive")] OrganizationViewModel organizationView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != organizationView.OrganizationId)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(organizationView.Name))
            {
                ViewBag.Message = "Organization name not given.";
                return View(organizationView);
            }

            var user = await _userManager.GetUserAsync(User);

            Organization organization = await _context.Organizations.Include(x => x.Tenant).FirstOrDefaultAsync(x => x.OrganizationId == num);

            organization.OrganizationId = organizationView.OrganizationId;
            organization.TenantId = organizationView.TenantId;
            organization.Name = organizationView.Name;
            organization.Address = organizationView.Address;
            organization.Email = organizationView.Email;
            organization.Phone = organizationView.Phone;
            organization.Website = organizationView.Website;
            organization.IsActive = organizationView.IsActive;
            organization.DateUpdated = DateTime.Now;
            organization.UpdatedBy = user.UserName;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListOrganizations));
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
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["TenantId"] = new SelectList(_context.Tenants, "TenantId", "Name", organization.TenantId);

            return View(organizationView);
        }

        public async Task<IActionResult> ListPayGrades()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.PayGrades.Where(x => x.OrganizationId == user.OrganizationId).ToListAsync());
        }

        public IActionResult AddPayGrade()
        {
            PayGradeViewModel payGrade = new ();

            return View(payGrade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayGrade([Bind("PayGradeId,OrganizationId,Name,Description")] PayGradeViewModel payGradeView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                payGradeView.OrganizationId = (int)user.OrganizationId;
            }

            PayGrade payGrade = new()
            {
                OrganizationId = payGradeView.OrganizationId,
                Name = payGradeView.Name,
                Description = payGradeView.Description,
                AddedBy = user.UserName
            };

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

            return View(payGradeView);
        }

        public async Task<IActionResult> EditPayGrade(string id)
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

            var payGrade = await _context.PayGrades.FirstOrDefaultAsync(x => x.PayGradeId == num);
            if (payGrade == null)
            {
                return NotFound();
            }

            PayGradeViewModel payGradeView = new()
            {
                PayGradeId = payGrade.PayGradeId,
                OrganizationId = payGrade.OrganizationId,
                Name = payGrade.Name,
                Description = payGrade.Description
            };

            return View(payGradeView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPayGrade(string id, [Bind("PayGradeId,OrganizationId,Name,Description")] PayGradeViewModel payGradeView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != payGradeView.PayGradeId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            PayGrade payGrade = new()
            {
                PayGradeId = payGradeView.PayGradeId,
                OrganizationId = payGradeView.OrganizationId,
                Name = payGradeView.Name,
                Description = payGradeView.Description,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payGrade);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListPayGrades));
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
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(payGradeView);
        }

        public async Task<IActionResult> ListPayGradeLevels(string gradeId)
        {
            int num = Resolver(gradeId);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            PayGrade payGrade = await _context.PayGrades.FirstOrDefaultAsync(x => x.PayGradeId == num);

            ViewData["gradeId"] = num;
            ViewData["gradeName"] = payGrade.Name;
            return View(await _context.PayGradeLevels.Include(x => x.PayGrade).Include(c => c.Currency).Where(x => x.PayGradeId == num).ToListAsync());
        }

        public async Task<IActionResult> AddPayGradeLevel(string gradeId)
        {
            int num = Resolver(gradeId);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            PayGrade payGrade = await _context.PayGrades.FirstOrDefaultAsync(x => x.PayGradeId == num);
            PayGradeLevelViewModel gradeLevel = new () { PayGradeId = num, PayGrade = payGrade };

            ViewData["gradeId"] = num;
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code");

            return View(gradeLevel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayGradeLevel([Bind("PayGradeLevelId,PayGradeId,PayGrade,Description,Level,BasicSalary,HourlyRate,OvertimeRate,CurrencyId,Currency")] PayGradeLevelViewModel payGradeLevelView)
        {
 
            var user = await _userManager.GetUserAsync(User);

            ViewData["gradeId"] = payGradeLevelView.PayGradeId;
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code", payGradeLevelView.CurrencyId);

            if (_context.PayGradeLevels.Any(x => x.PayGradeId == payGradeLevelView.PayGradeId && x.Level == payGradeLevelView.Level))
            {
                ViewBag.Message = "Pay grade level can't be duplicated. ";
                return View(payGradeLevelView);
            }

            PayGradeLevel gradeLevel = new()
            {
                PayGradeId = payGradeLevelView.PayGradeId,
                Description = payGradeLevelView.Description,
                Level = payGradeLevelView.Level,
                BasicSalary = payGradeLevelView.BasicSalary,
                HourlyRate = payGradeLevelView.HourlyRate,
                OvertimeRate = payGradeLevelView.OvertimeRate,
                CurrencyId = payGradeLevelView.CurrencyId,
                AddedBy = user.UserName
            };

            if (ModelState.IsValid)
            {
                await _context.AddAsync(gradeLevel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListPayGradeLevels), new { gradeId = _protector.Encode(payGradeLevelView.PayGradeId.ToString()) });
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }          

            return View(payGradeLevelView);
        }

        public async Task<IActionResult> EditPayGradeLevel(string id, string gradeId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id == null)
            {
                return NotFound();
            }

            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            int num2 = Resolver(gradeId);
            if (num2 == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var gradeLevel = await _context.PayGradeLevels.Include(g => g.PayGrade).Include(s => s.Currency).Where(x => x.PayGradeId == num2).FirstOrDefaultAsync(x => x.PayGradeLevelId == num);
            if (gradeLevel == null)
            {
                return NotFound();
            }

            PayGradeLevelViewModel payGradeLevelView = new()
            {
                PayGradeLevelId = num,
                PayGradeId = num2,
                Description = gradeLevel.Description,
                Level = gradeLevel.Level,
                BasicSalary = gradeLevel.BasicSalary,
                HourlyRate = gradeLevel.HourlyRate,
                OvertimeRate = gradeLevel.OvertimeRate,
                CurrencyId = gradeLevel.CurrencyId,
                PayGrade = gradeLevel.PayGrade,
                Currency = gradeLevel.Currency,
            };

            ViewData["gradeId"] = num2;
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code", gradeLevel.CurrencyId);

            return View(payGradeLevelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPayGradeLevel(string id, [Bind("PayGradeLevelId,PayGradeId,Description,Level,BasicSalary,HourlyRate,OvertimeRate,CurrencyId")] PayGradeLevelViewModel payGradeLevelView)
        {
            var user = await _userManager.GetUserAsync(User);

            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != payGradeLevelView.PayGradeLevelId)
            {
                return NotFound();
            }

            PayGradeLevel gradeLevel = new()
            {
                PayGradeLevelId = payGradeLevelView.PayGradeLevelId,
                PayGradeId = payGradeLevelView.PayGradeId,
                Description = payGradeLevelView.Description,
                Level = payGradeLevelView.Level,
                BasicSalary = payGradeLevelView.BasicSalary,
                HourlyRate = payGradeLevelView.HourlyRate,
                OvertimeRate = payGradeLevelView.OvertimeRate,
                CurrencyId = payGradeLevelView.CurrencyId,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now,
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradeLevel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListPayGradeLevels), new { gradeId = _protector.Encode(payGradeLevelView.PayGradeId.ToString()) });
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
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["PayGradeId"] = new SelectList(_context.PayGrades.Where(x => x.OrganizationId == user.OrganizationId), "PayGradeId", "Name", gradeLevel.PayGradeId);
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Code", gradeLevel.CurrencyId);

            return View(gradeLevel);
        }

        //public async Task<IActionResult> ListPayGradeLevelDeductions(string levelId)
        //{
        //    int num = Resolver(levelId);
        //    if (num == 0)
        //    {
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    return View(await _context.PayGradeLevelDeductions.Include(x => x.PayGradeLevel).Where(x => x.PayGradeLevelId == num).ToListAsync());
        //}

        //public IActionResult AddPayGradeLevelDeduction(string levelId)
        //{
        //    int num = Resolver(levelId);
        //    if (num == 0)
        //    {
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    PayGradeLevelDeduction payGradeLevelDeduction = new() { PayGradeLevelId = num };

        //    ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels, "PayGradeLevelId", "Description");

        //    return View(payGradeLevelDeduction);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddPayGradeLevelDeduction(string levelId, [Bind("PayGradeLevelDeductionId,PayGradeLevelId,TaxRate,HealthInsurance,RetirementContribution")] PayGradeLevelDeduction gradeLevelDeduction)
        //{
        //    int num = Resolver(levelId);
        //    if (num == 0)
        //    {
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    var user = await _userManager.GetUserAsync(User);

        //    gradeLevelDeduction.AddedBy = user.UserName;

        //    if (ModelState.IsValid)
        //    {
        //        await _context.AddAsync(gradeLevelDeduction);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(ListPayGradeLevelDeductions));
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Unable to save changes. " +
        //            "Try again, and if the problem persists, " +
        //            "see your system administrator.";
        //    }

        //    ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels.Where(x => x.PayGradeLevelId == num), "PayGradeLevelId", "Description", gradeLevelDeduction.PayGradeLevelId);

        //    return View(gradeLevelDeduction);
        //}

        //public async Task<IActionResult> EditPayGradeLevelDeduction(string id)
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

        //    var gradeLevelDeduction = await _context.PayGradeLevelDeductions.Include(g => g.PayGradeLevel).FirstOrDefaultAsync(x => x.PayGradeLevelDeductionId == num);
        //    if (gradeLevelDeduction == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels, "PayGradeLevelId", "Description", gradeLevelDeduction.PayGradeLevelId);

        //    return View(gradeLevelDeduction);
        //}

        //public async Task<IActionResult> EditPayGradeLevelDeduction(string id, [Bind("PayGradeLevelDeductionId,PayGradeLevelId,TaxRate,HealthInsurance,RetirementContribution")] PayGradeLevelDeduction gradeLevelDeduction)
        //{
        //    int num = Resolver(id);
        //    if (num == 0)
        //    {
        //        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //    }

        //    if (num != gradeLevelDeduction.PayGradeLevelDeductionId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(gradeLevelDeduction);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException ex)
        //        {
        //            if (!PayGradeLevelDeductionExists(gradeLevelDeduction.PayGradeLevelDeductionId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
        //                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
        //            }
        //        }
        //        return RedirectToAction(nameof(ListPayGradeLevelDeductions));
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Unable to save changes. " +
        //            "Try again, and if the problem persists, " +
        //            "see your system administrator.";
        //    }

        //    ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels, "PayGradeLevelId", "Description", gradeLevelDeduction.PayGradeLevelId);

        //    return View(gradeLevelDeduction);
        //}

        public async Task<IActionResult> ListSubscriptions()
        {
            return View(await _context.Subscriptions.Include(g => g.Organization).Include(s => s.SubscriptionRate).ToListAsync());
        }

        public IActionResult AddSubscription()
        {

            Subscription subscription = new() 
            {
                StartDate = startDate,
                EndDate = endDate
            };

            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Name");
            ViewData["SubscriptionRateId"] = new SelectList(_context.SubscriptionsRates, "SubscriptionRateId", "Type");

            return View(subscription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubscription([Bind("SubscriptionId,OrganizationId,SubscriptionRateId,StartDate,EndDate,Status")] Subscription subscription)
        {
            if (subscription.EndDate < subscription.StartDate)
            {
                ViewBag.Message = "Wrong dates choices. Pleae enter dates correctly and try again.";
                return View(subscription);
            }

            if (ModelState.IsValid)
            {
                await _context.AddAsync(subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListSubscriptions));
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

            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var subscription = await _context.Subscriptions.Include(g => g.Organization).Include(s => s.SubscriptionRate).FirstOrDefaultAsync(x => x.SubscriptionRateId == num);
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
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != subscription.SubscriptionId)
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
                return RedirectToAction(nameof(ListSubscriptions));
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

        public async Task<IActionResult> ListSubscriptionRates()
        {
            return View(await _context.SubscriptionsRates.Include(c => c.Currency).ToListAsync());
        }

        public IActionResult AddSubscriptionRate()
        {
            SubscriptionRate subscriptionRate = new ();

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Name");

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
                return RedirectToAction(nameof(ListSubscriptionRates));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Name", subscriptionRate.CurrencyId);

            return View(subscriptionRate);
        }

        public async Task<IActionResult> EditSubscriptionRate(string id)
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

            var subscriptionRate = await _context.SubscriptionsRates.Include(s => s.Currency).FirstOrDefaultAsync(x => x.SubscriptionRateId == num);
            if (subscriptionRate == null)
            {
                return NotFound();
            }

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Name", subscriptionRate.CurrencyId);

            return View(subscriptionRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubscriptionRate(string id, [Bind("SubscriptionRateId,Type,Period,Amount,CurrencyId,NumberOfEmployees,NumberOfClients")] SubscriptionRate subscriptionRate)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != subscriptionRate.SubscriptionRateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscriptionRate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListSubscriptionRates));
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
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "Name", subscriptionRate.CurrencyId);

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
            var user = await _userManager.GetUserAsync(User);

            if (string.IsNullOrWhiteSpace(tenant.Name))
            {
                ViewBag.Message = "Tenant name not given.";
                return View(tenant);
            }

            string tenantName = Utils.RemoveSpecialCharacters2(tenant.Name);

            if (_context.Tenants.Any(x => x.Name.Equals(tenantName)))
            {
                ViewBag.Message = "Tenant name is not unique.";
                return View(tenant);
            }

            tenant.Name = tenantName;
            tenant.ApiKey = Guid.NewGuid();
            tenant.DateAdded = DateTime.Now;
            tenant.AddedBy = user.UserName;

            try
            {
                await _context.AddAsync(tenant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListTenants));
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, "An error has occurred saving item", ex);

                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    $"see your system administrator.";
            }

            return View(tenant);
        }

        public async Task<IActionResult> EditTenant(string id)
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

            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.TenantId == num);
            if (tenant == null)
            {
                return NotFound();
            }

            TenantViewModel tenantView = new()
            {
                TenantId = tenant.TenantId,
                Name = tenant.Name,
                IsActive = tenant.IsActive,
            };

            return View(tenantView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTenant(string id, [Bind("TenantId,Name,IsActive")] TenantViewModel tenantView)
        {
            var user = await _userManager.GetUserAsync(User);           

            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != tenantView.TenantId)
            {
                return NotFound();
            }

            Tenant tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.TenantId == num);

            tenant.TenantId = tenantView.TenantId;
            tenant.IsActive = tenantView.IsActive;
            tenant.DateUpdated = DateTime.Now;
            tenant.UpdatedBy = user.UserName;


            try
            {
                _context.Update(tenant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListTenants));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TenantExists(tenant.TenantId))
                {
                    return NotFound();
                }
                else
                {
                    _logger.Log(LogLevel.Error, "An error has occurred saving item", ex);

                    ViewBag.Message = "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.";
                }
            }
                            
            return View(tenantView);
        }

        public async Task<IActionResult> ListVendors()
        {
            return View(await _context.Vendors.ToListAsync());
        }

        public IActionResult AddVendor()
        {
            VendorViewModel vendor = new();
            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVendor([Bind("VendorId,OrganizationId,Name,Email,Phone,Address,City,PostCode,ContactPerson,PaymentMethod,BankName,BankCode,TransitCode,AccountNumber")] VendorViewModel vendorView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                vendorView.OrganizationId = (int)user.OrganizationId;
            }

            if (vendorView.PaymentMethod == PaymentMethod.DirectDeposit)
            {
                if (string.IsNullOrEmpty(vendorView.BankName))
                {
                    ViewBag.Message = "Bank name is required for a direct deposit.";
                    return View(vendorView);
                }

                if (string.IsNullOrEmpty(vendorView.BankCode))
                {
                    ViewBag.Message = "Bank code is required for a direct deposit.";
                    return View(vendorView);
                }

                if (string.IsNullOrEmpty(vendorView.TransitCode))
                {
                    ViewBag.Message = "Transit code is required for a direct deposit.";
                    return View(vendorView);
                }

                if (string.IsNullOrEmpty(vendorView.AccountNumber))
                {
                    ViewBag.Message = "Account number is required for a direct deposit.";
                    return View(vendorView);
                }
            }

            Vendor vendor = new()
            {
                VendorId = vendorView.VendorId,
                OrganizationId = vendorView.OrganizationId,
                Name = vendorView.Name,
                Email = vendorView.Email,
                Phone = vendorView.Phone,
                Address = vendorView.Address,
                City = vendorView.City,
                PostCode = vendorView.PostCode,
                ContactPerson = vendorView.ContactPerson,
                PaymentMethod = vendorView.PaymentMethod,
                BankCode = vendorView.BankCode,
                BankName = vendorView.BankName,
                TransitCode = vendorView.TransitCode,
                AccountNumber = vendorView.AccountNumber,
                AddedBy = user.UserName
            };

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

            return View(vendorView);
        }

        public async Task<IActionResult> EditVendor(string id)
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

            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.VendorId == num);
            if (vendor == null)
            {
                return NotFound();
            }

            VendorViewModel vendorView = new()
            {
                VendorId = vendor.VendorId,
                OrganizationId = vendor.OrganizationId,
                Name = vendor.Name,
                Email = vendor.Email,
                Phone = vendor.Phone,
                Address = vendor.Address,
                City = vendor.City,
                PostCode = vendor.PostCode,
                ContactPerson = vendor.ContactPerson,
                PaymentMethod = vendor.PaymentMethod,
                BankCode = vendor.BankCode,
                BankName = vendor.BankName,
                TransitCode = vendor.TransitCode,
                AccountNumber = vendor.AccountNumber
            };

            return View(vendorView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVendor(string id, [Bind("VendorId,OrganizationId,Name,Email,Phone,Address,City,PostCode,ContactPerson,PaymentMethod,BankName,BankCode,TransitCode,AccountNumber")] VendorViewModel vendorView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != vendorView.VendorId)
            {
                return NotFound();
            }

            if (vendorView.PaymentMethod == PaymentMethod.DirectDeposit)
            {
                if (string.IsNullOrEmpty(vendorView.BankName))
                {
                    ViewBag.Message = "Bank name is required for a direct deposit.";
                    return View(vendorView);
                }

                if (string.IsNullOrEmpty(vendorView.BankCode))
                {
                    ViewBag.Message = "Bank code is required for a direct deposit.";
                    return View(vendorView);
                }

                if (string.IsNullOrEmpty(vendorView.TransitCode))
                {
                    ViewBag.Message = "Transit code is required for a direct deposit.";
                    return View(vendorView);
                }

                if (string.IsNullOrEmpty(vendorView.AccountNumber))
                {
                    ViewBag.Message = "Account number is required for a direct deposit.";
                    return View(vendorView);
                }
            }

            var user = await _userManager.GetUserAsync(User);

            Vendor vendor = new()
            {
                VendorId = vendorView.VendorId,
                OrganizationId = vendorView.OrganizationId,
                Name = vendorView.Name,
                Email = vendorView.Email,
                Phone = vendorView.Phone,
                Address = vendorView.Address,
                City = vendorView.City,
                PostCode = vendorView.PostCode,
                ContactPerson = vendorView.ContactPerson,
                PaymentMethod = vendorView.PaymentMethod,
                BankCode = vendorView.BankCode,
                BankName = vendorView.BankName,
                TransitCode = vendorView.TransitCode,
                AccountNumber = vendorView.AccountNumber,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ListVendors));
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
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.";
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(vendorView);
        }

        public async Task<IActionResult> SmtpSettings(string id)
        {
            ViewData["Title"] = "SMTP Settings";
            ViewData["Page"] = "SMTP Settings";

            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var smtpSettings = await _context.SmtpSettings.Where(x => x.OrganizationId == num).FirstOrDefaultAsync();

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

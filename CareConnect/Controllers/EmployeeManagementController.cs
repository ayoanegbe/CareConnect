using CareConnect.Data;
using CareConnect.Enums;
using CareConnect.Interfaces;
using CareConnect.Models;
using CareConnect.Models.CareConnectViewModels;
using CareConnect.Services;
using GroupDocs.Viewer.Options;
using GroupDocs.Viewer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.IO;

namespace CareConnect.Controllers
{
    public class EmployeeManagementController : Controller
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

        public EmployeeManagementController(
            ILogger<EmployeeManagementController> logger,
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
        public async Task<IActionResult> Index(string id) // pass organization ID
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            return View(
                await _context.Employees.Where(x => x.OrganizationId == num)
                .Include(d => d.Department)
                .Include(j => j.JobTitle)
                .ToListAsync());
        }

        public async Task<IActionResult> ListEmployees()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(
                await _context.Employees
                .Where(x => x.OrganizationId == user.OrganizationId)
                .Include(d => d.Department)
                .Include(g => g.JobTitle)
                .Include(p => p.PayGradeLevel)
                .ToListAsync()
                );
        }

        public async Task<IActionResult> AddEmployee()
        {
            var user = await _userManager.GetUserAsync(User);

            EmployeeViewModel employee = new()
            {
                BirthDate = DateTime.Now.AddYears(-18),
                Documents = new List<EmployeeDocument>()
            };

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == user.OrganizationId), "DepartmentId", "Name");
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == user.OrganizationId), "JobTitleId", "Title");
            ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels.Where(x => x.PayGrade.OrganizationId == user.OrganizationId), "PayGradeLevelId", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees.Where(x => x.OrganizationId == user.OrganizationId), "EmployeeId", "FullName");

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee([Bind("EmployeeId,DepartmentId,JobTitleId,PayGradeLevelId,FirstName,MiddleName,LastName,Gender,BirthDate,Address,City,PostCode,Phone,Email,EmergencyContactPhone,EmergencyContactAddress,Relationship,DateJoined,Status,ContractorType,ImmigrationStatus,SocialInsuranceNumber,LineManagerId,PaymentMethod,BankName,BankCode,TransitCode,AccountNumber,PaymentFrequency,Documents")] EmployeeViewModel employeeView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                employeeView.OrganizationId = (int)user.OrganizationId;
            }

            if (employeeView.Status == EmployeeType.Contract)
            {
                if (string.IsNullOrEmpty(employeeView.ContractorType.ToString()))
                {
                    ViewBag.Message = "Contract Type is required!";
                    return View(employeeView);
                }
            }

            if (employeeView.PaymentMethod == PaymentMethod.DirectDeposit)
            {
                if (string.IsNullOrEmpty(employeeView.BankName))
                {
                    ViewBag.Message = "Bank name is required for a direct deposit.";
                    return View(employeeView);
                }

                if (string.IsNullOrEmpty(employeeView.BankCode))
                {
                    ViewBag.Message = "Bank code is required for a direct deposit.";
                    return View(employeeView);
                }

                if (string.IsNullOrEmpty(employeeView.TransitCode))
                {
                    ViewBag.Message = "Transit code is required for a direct deposit.";
                    return View(employeeView);
                }

                if (string.IsNullOrEmpty(employeeView.AccountNumber))
                {
                    ViewBag.Message = "Account number is required for a direct deposit.";
                    return View(employeeView);
                }
            }

            Employee employee = new()
            {
                OrganizationId = employeeView.OrganizationId,
                DepartmentId = employeeView.DepartmentId,
                JobTitleId = employeeView.JobTitleId,
                PayGradeLevelId = employeeView.PayGradeLevelId,
                FirstName = employeeView.FirstName,
                MiddleName = employeeView.MiddleName,
                LastName = employeeView.LastName,
                Gender = employeeView.Gender,
                BirthDate = employeeView.BirthDate,
                Address = employeeView.Address,
                City = employeeView.City,
                PostCode = employeeView.PostCode,
                Phone = employeeView.Phone,
                Email = employeeView.Email,
                EmergencyContactPhone = employeeView.EmergencyContactPhone,
                EmergencyContactAddress = employeeView.EmergencyContactAddress,
                Relationship = employeeView.Relationship,
                DateJoined = employeeView.DateJoined,
                Status = employeeView.Status,
                ContractorType = employeeView.ContractorType,
                ImmigrationStatus = employeeView.ImmigrationStatus,
                SocialInsuranceNumber = employeeView.SocialInsuranceNumber,
                LineManagerId = employeeView.LineManagerId,
                PaymentMethod = employeeView.PaymentMethod,
                BankName = employeeView.BankName,
                BankCode = employeeView.BankCode,
                TransitCode = employeeView.TransitCode,
                AccountNumber = employeeView.AccountNumber,
                PaymentFrequency = employeeView.PaymentFrequency,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                await _context.AddAsync(employee);
                await _context.SaveChangesAsync();

                var newValue = JsonConvert.SerializeObject(employee);

                await _audit.UpdateAuditTrail((int)user.OrganizationId, employee.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                return RedirectToAction(nameof(ListEmployees));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", employee.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", employee.JobTitleId);
            ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels.Where(x => x.PayGrade.OrganizationId == user.OrganizationId), "PayGradeLevelId", "Description", employee.PayGradeLevelId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees.Where(x => x.OrganizationId == user.OrganizationId && x.EmployeeId != employeeView.EmployeeId), "EmployeeId", "FullName", employee.LineManagerId);

            return View(employeeView);
        }

        public async Task<IActionResult> EditEmployee(string id)
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

            var employee = await _context.Employees.Include(m => m.Department).Include(t => t.JobTitle).Include(p => p.PayGradeLevel).FirstOrDefaultAsync(x => x.EmployeeId == num);
            if (employee == null)
            {
                return NotFound();
            }

            EmployeeViewModel employeeView = new()
            {
                EmployeeId = employee.EmployeeId,
                OrganizationId = employee.OrganizationId,
                DepartmentId = employee.DepartmentId,
                JobTitleId = employee.JobTitleId,
                PayGradeLevelId = employee.PayGradeLevelId,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                BirthDate = employee.BirthDate,
                Address = employee.Address,
                City = employee.City,
                PostCode = employee.PostCode,
                Phone = employee.Phone,
                Email = employee.Email,
                EmergencyContactPhone = employee.EmergencyContactPhone,
                EmergencyContactAddress = employee.EmergencyContactAddress,
                Relationship = employee.Relationship,
                DateJoined = employee.DateJoined,
                Status = employee.Status,
                ContractorType = employee.ContractorType,
                ImmigrationStatus = employee.ImmigrationStatus,
                SocialInsuranceNumber = employee.SocialInsuranceNumber,
                LineManagerId = employee.LineManagerId,
                PaymentMethod = employee.PaymentMethod,
                BankName = employee.BankName,
                BankCode = employee.BankCode,
                TransitCode = employee.TransitCode,
                AccountNumber = employee.AccountNumber,
                PaymentFrequency = employee.PaymentFrequency,
            };

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", employee.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", employee.JobTitleId);
            ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels.Where(x => x.PayGrade.OrganizationId == user.OrganizationId), "PayGradeLevelId", "Description", employee.PayGradeLevelId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees.Where(x => x.OrganizationId == user.OrganizationId && x.EmployeeId != employeeView.EmployeeId), "EmployeeId", "FullName", employee.LineManagerId);

            return View(employeeView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(string id, [Bind("EmployeeId,OrganizationId,DepartmentId,JobTitleId,PayGradeLevelId,FirstName,MiddleName,LastName,Gender,BirthDate,Address,City,PostCode,Phone,Email,EmergencyContactPhone,EmergencyContactAddress,Relationship,DateJoined,Status,ContractorType,ImmigrationStatus,SocialInsuranceNumber,LineManagerId,PaymentMethod,BankName,BankCode,TransitCode,AccountNumber,PaymentFrequency,FormFiles")] EmployeeViewModel employeeView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != employeeView.EmployeeId)
            {
                return NotFound();
            }

            if (employeeView.Status == EmployeeType.Contract)
            {
                if (string.IsNullOrEmpty(employeeView.ContractorType.ToString()))
                {
                    ViewBag.Message = "Specify Contract Type for this contractor!";
                    return View(employeeView);
                }
            }

            if (employeeView.PaymentMethod == PaymentMethod.DirectDeposit)
            {
                if (string.IsNullOrEmpty(employeeView.BankName))
                {
                    ViewBag.Message = "Bank name is required for a direct deposit.";
                    return View(employeeView);
                }

                if (string.IsNullOrEmpty(employeeView.BankCode))
                {
                    ViewBag.Message = "Bank code is required for a direct deposit.";
                    return View(employeeView);
                }

                if (string.IsNullOrEmpty(employeeView.TransitCode))
                {
                    ViewBag.Message = "Transit code is required for a direct deposit.";
                    return View(employeeView);
                }

                if (string.IsNullOrEmpty(employeeView.AccountNumber))
                {
                    ViewBag.Message = "Account number is required for a direct deposit.";
                    return View(employeeView);
                }
            }

            var user = await _userManager.GetUserAsync(User);

            Employee employee = new()
            {
                OrganizationId = employeeView.OrganizationId,
                DepartmentId = employeeView.DepartmentId,
                JobTitleId = employeeView.JobTitleId,
                PayGradeLevelId = employeeView.PayGradeLevelId,
                FirstName = employeeView.FirstName,
                MiddleName = employeeView.MiddleName,
                LastName = employeeView.LastName,
                Gender = employeeView.Gender,
                BirthDate = employeeView.BirthDate,
                Address = employeeView.Address,
                City = employeeView.City,
                PostCode = employeeView.PostCode,
                Phone = employeeView.Phone,
                Email = employeeView.Email,
                EmergencyContactPhone = employeeView.EmergencyContactPhone,
                EmergencyContactAddress = employeeView.EmergencyContactAddress,
                Relationship = employeeView.Relationship,
                DateJoined = employeeView.DateJoined,
                Status = employeeView.Status,
                ContractorType = employeeView.ContractorType,
                ImmigrationStatus = employeeView.ImmigrationStatus,
                SocialInsuranceNumber = employeeView.SocialInsuranceNumber,
                LineManagerId = employeeView.LineManagerId,
                PaymentMethod = employeeView.PaymentMethod,
                BankName = employeeView.BankName,
                BankCode = employeeView.BankCode,
                TransitCode = employeeView.TransitCode,
                AccountNumber = employeeView.AccountNumber,
                PaymentFrequency = employeeView.PaymentFrequency,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employee.EmployeeId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ChangeTracker.Clear();
                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(employee);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, employee.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);

                    return RedirectToAction(nameof(ListEmployees));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", employee.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", employee.JobTitleId);
            ViewData["PayGradeLevelId"] = new SelectList(_context.PayGradeLevels.Where(x => x.PayGrade.OrganizationId == user.OrganizationId), "PayGradeLevelId", "Description", employee.PayGradeLevelId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees.Where(x => x.OrganizationId == user.OrganizationId && x.EmployeeId != employeeView.EmployeeId), "EmployeeId", "FullName", employee.LineManagerId);

            return View(employeeView);
        }

        public async Task<IActionResult> EmployeeDocs(string id)
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

            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == num);
            List<EmployeeDocument> docs = await _context.EmployeeDocuments.Where(x => x.EmployeeId == num).ToListAsync();

            EmployeeViewModel employeeView = new()
            {
                EmployeeId = employee.EmployeeId,
                OrganizationId = employee.OrganizationId,
                DepartmentId = employee.DepartmentId,
                JobTitleId = employee.JobTitleId,
                PayGradeLevelId = employee.PayGradeLevelId,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                BirthDate = employee.BirthDate,
                Address = employee.Address,
                City = employee.City,
                PostCode = employee.PostCode,
                Phone = employee.Phone,
                Email = employee.Email,
                EmergencyContactPhone = employee.EmergencyContactPhone,
                EmergencyContactAddress = employee.EmergencyContactAddress,
                Relationship = employee.Relationship,
                DateJoined = employee.DateJoined,
                Status = employee.Status,
                ContractorType = employee.ContractorType,
                ImmigrationStatus = employee.ImmigrationStatus,
                SocialInsuranceNumber = employee.SocialInsuranceNumber,
                LineManagerId = employee.LineManagerId,
                PaymentMethod = employee.PaymentMethod,
                BankName = employee.BankName,
                BankCode = employee.BankCode,
                TransitCode = employee.TransitCode,
                AccountNumber = employee.AccountNumber,
                Documents = docs
            };

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", employee.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", employee.JobTitleId);

            return View(employeeView);
        }
       
        public async Task<IActionResult> AddDocument(string id)
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

            EmployeeDocumentViewModel document = new()
            {
                EmployeeId = num
            };

            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDocument(string id, [Bind("EmployeeDocumentId,EmployeeId,DocumentName,FilePath,File")] EmployeeDocumentViewModel documentView)
        {
            var user = await _userManager.GetUserAsync(User);
            var filePath = string.Empty;

            if (id == null)
            {
                return NotFound();
            }

            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var fileName = Path.GetFileName(documentView.File.FileName);

            if ((documentView.File.Length > 0) && ((fileName.EndsWith(".doc")) || (fileName.EndsWith(".docx")) || (fileName.EndsWith(".pdf"))))
            {
                filePath = await _fileService.SaveFile2(documentView.File, "Employees");
                if (string.IsNullOrEmpty(filePath))
                {
                    ViewBag.Message = "File could not be saved. If problem persists, please consult your Administrator!";
                    return View(documentView);
                }                 
            }
            else
            {
                ViewBag.Message = "File empty or file type not supported!";
                return View(documentView);
            }

            EmployeeDocument employeeDocument = new()
            {
                EmployeeId = documentView.EmployeeId,
                DocumentName = documentView.DocumentName,
                FilePath = filePath,
                AddedBy = user.UserName,
                DateAdded = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                await _context.AddAsync(employeeDocument);
                await _context.SaveChangesAsync();

                var newValue = JsonConvert.SerializeObject(employeeDocument);

                await _audit.UpdateAuditTrail((int)user.OrganizationId, employeeDocument.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                return RedirectToAction(nameof(EmployeeDocs), new {id = _protector.Encode(documentView.EmployeeId.ToString())});
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(documentView);
        }

        public async Task<IActionResult> DocView(string id)
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

            EmployeeDocument employeeDocument = await _context.EmployeeDocuments.FirstOrDefaultAsync(x => x.EmployeeDocumentId == num);

            MemoryStream outputStream = new ();

            FileType fileType = FileType.FromExtension(Path.GetExtension(employeeDocument.FilePath));

            using (Viewer viewer = new(() => GetSourceFileStream(employeeDocument.FilePath), () => new LoadOptions(fileType)))
            {
                HtmlViewOptions options = HtmlViewOptions.ForEmbeddedResources(
                    (pageNumber) => outputStream,
                    (pageNumber, pageStream) => { });

                viewer.View(options);
                
            }

            outputStream.Position = 0;

            return File(outputStream, "text/html");
        }

        private Stream GetSourceFileStream(string fileName) =>
            new MemoryStream(GetSourceFileBytesFromDb(fileName));

        private static byte[] GetSourceFileBytesFromDb(string fileName) =>
            System.IO.File.ReadAllBytes(fileName);

        [AllowAnonymous]
        public async Task<IActionResult> ListVacancies()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(await _context.Vacancies.Where(x => x.OrganizationId == user.OrganizationId).Include(d => d.Department).Include(t => t.JobTitle).ToListAsync());
        }
        
        public async Task<IActionResult> AddVacancy()
        {
            var user = await _userManager.GetUserAsync(User);

            VacancyViewModel vacancy = new() { ClosingDate = DateTime.Today.AddDays(7) };

            if (user.OrganizationId != null)
            {
                vacancy.OrganizationId = (int)user.OrganizationId;
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == user.OrganizationId), "DepartmentId", "Name");
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == user.OrganizationId), "JobTitleId", "Title");

            return View(vacancy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVacancy([Bind("VacancyId,JobTitleId,DepartmentId,JobDescription,Requirements,Status,ClosingDate")] VacancyViewModel vacancyView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                vacancyView.OrganizationId = (int)user.OrganizationId;
            }

            Vacancy vacancy = new()
            {
                VacancyId = vacancyView.VacancyId,
                OrganizationId = vacancyView.OrganizationId,
                JobDescription = vacancyView.JobDescription,
                JobTitleId = vacancyView.JobTitleId,
                DepartmentId = vacancyView.DepartmentId,
                Requirements = vacancyView.Requirements,
                Status = vacancyView.Status,
                ClosingDate = vacancyView.ClosingDate,
                AddedBy = user.UserName
            };

            if (ModelState.IsValid)
            {
                await _context.AddAsync(vacancy);
                await _context.SaveChangesAsync();

                var newValue = JsonConvert.SerializeObject(vacancy);

                await _audit.UpdateAuditTrail((int)user.OrganizationId, vacancy.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                return RedirectToAction(nameof(ListVacancies));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", vacancy.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", vacancy.JobTitleId);

            return View(vacancyView);
        }

        public async Task<IActionResult> EditVacancy(string id)
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

            var vacancy = await _context.Vacancies.Include(m => m.Department).Include(t => t.JobTitle).FirstOrDefaultAsync(x => x.VacancyId == num);
            if (vacancy == null)
            {
                return NotFound();
            }

            VacancyViewModel vacancyViewModel = new()
            {
                VacancyId = vacancy.VacancyId,
                OrganizationId = vacancy.OrganizationId,
                JobDescription = vacancy.JobDescription,
                JobTitleId = vacancy.JobTitleId,
                DepartmentId = vacancy.DepartmentId,
                Requirements = vacancy.Requirements,
                Status = vacancy.Status,
                ClosingDate = vacancy.ClosingDate
            };

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", vacancy.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", vacancy.JobTitleId);

            return View(vacancyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVacancy(string id, [Bind("VacancyId,JobTitleId,OrganizationId,DepartmentId,JobDescription,Requirements,Status,ClosingDate")] VacancyViewModel vacancyView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != vacancyView.VacancyId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            Vacancy vacancy = new()
            {
                VacancyId = vacancyView.VacancyId,
                OrganizationId = vacancyView.OrganizationId,
                JobDescription = vacancyView.JobDescription,
                JobTitleId = vacancyView.JobTitleId,
                DepartmentId = vacancyView.DepartmentId,
                Requirements = vacancyView.Requirements,
                Status = vacancyView.Status,
                ClosingDate = vacancyView.ClosingDate,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Vacancies.FirstOrDefaultAsync(x => x.VacancyId == vacancy.VacancyId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ChangeTracker.Clear();
                    _context.Update(vacancy);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(vacancy);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, vacancy.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);

                    return RedirectToAction(nameof(ListVacancies));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!VacancyExists(vacancy.VacancyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
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

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", vacancy.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", vacancy.JobTitleId);

            return View(vacancyView);
        }

        [AllowAnonymous]
        public IActionResult Apply()
        {
            ApplicantViewModel model = new();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply([Bind("ApplicantId,VacancyId,OrganizationId,FirstName,MiddleName,LastName,Email,ResumePath,CoverLetterPath,ResumeFile,CoverLetterFile,Token")] ApplicantViewModel model)
        {
            var verifiedRecaptcha = _recaptchaService.VerifyRecaptcha(model.Token);

            if (!verifiedRecaptcha.Result.success && verifiedRecaptcha.Result.score <= 0.5)
            {
                ModelState.AddModelError(string.Empty, "You failed the CAPTCHA.");
                ViewBag.Message = "You failed the CAPTCHA!";
                return View(model);
            }

            string strResumePath = string.Empty;
            string strCoverPath = string.Empty;

            if (!DnsCheck.IsEmailValid(model.Email).Result)
            {
                ViewBag.Message = "Invalid e-mail address!";
                return View(model);
            }

            if (!Utils.IsNumeric(model.Phone))
            {
                ViewBag.Message = "Invalid character(s) in phone number!";
                return View(model);
            }

            if (model.ResumeFile == null)
            {
                ViewBag.Message = "Resume not attached!";
                return View(model);
            }

            if (model.CoverLetterFile == null)
            {
                ViewBag.Message = "Cover letter not attached!";
                return View(model);
            }

            DateTime fileDate = DateTime.Now;

            var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var uploads = Path.Combine(webRoot, "documents");

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var resumeFileName = Path.GetFileName(model.ResumeFile.FileName);

            var resumeFilePath = Path.Combine(uploads, resumeFileName);

            //long resumeFileSize = new FileInfo(resumeFilePath).Length;

            var coverFileName = Path.GetFileName(model.CoverLetterFile.FileName);

            var coverFilePath = Path.Combine(uploads, coverFileName);

            //long coverFileSize = new FileInfo(coverFilePath).Length;

            try
            {
                if ((model.ResumeFile.Length > 0) && ((resumeFileName.EndsWith(".doc")) || (resumeFileName.EndsWith(".docx")) || (resumeFileName.EndsWith(".pdf"))))
                {
                    using FileStream fileStream = new(resumeFilePath, FileMode.Create);                    

                    try
                    {
                        await model.ResumeFile.CopyToAsync(fileStream);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Unexpected error! " + ex.Message;
                        return View(model);
                    }
                    
                    var ext = Path.GetExtension(model.ResumeFile.FileName);
                    var newFileName = $"CV_{model.FirstName}_{model.LastName}_{fileDate:yyyy-MM-dd-HH-mm-ss}";
                    newFileName = _fileService.CheckInvalidChars(newFileName);
                    newFileName += ext;
                    var newFilePath = Path.Combine(uploads, newFileName);
                    System.IO.File.Move(resumeFilePath, newFilePath);
                    resumeFileName = newFileName;
                    strResumePath = newFilePath;
                }
                else
                {
                    ViewBag.Message = "File empty or file type not supported!";
                    return View(model);
                }

                if ((model.CoverLetterFile.Length > 0) && ((coverFileName.EndsWith(".doc")) || (coverFileName.EndsWith(".docx")) || (coverFileName.EndsWith(".pdf"))))
                {
                    using FileStream fileStream = new(coverFilePath, FileMode.Create);

                    try
                    {
                        await model.CoverLetterFile.CopyToAsync(fileStream);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Unexpected error! " + ex.Message;
                        return View(model);
                    }

                    var ext = Path.GetExtension(model.CoverLetterFile.FileName);
                    var newFileName = $"CL_{model.FirstName}_{model.LastName}_{fileDate:yyyy-MM-dd-HH-mm-ss}";
                    newFileName = _fileService.CheckInvalidChars(newFileName);
                    newFileName += ext;
                    var newFilePath = Path.Combine(uploads, newFileName);
                    System.IO.File.Move(coverFilePath, newFilePath);
                    coverFileName = newFileName;
                    strCoverPath = newFilePath;
                }
                else
                {
                    ViewBag.Message = "File empty or file type not supported!";
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Unexpected error! " + ex.Message;
                return View(model);
            }

            Applicant applicant = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Email = model.Email,
                Phone = model.Phone,
                ResumePath = Path.Combine(uploads, resumeFileName),
                CoverLetterPath = Path.Combine(uploads, coverFileName)
            };

            try
            {
                _context.Add(applicant);
                await _context.SaveChangesAsync();

                //await _emailSender.SendEmailAsync(_emailSettings.Resume, career.FirstName + " " + career.LastName,
                //        MsgBody(career), strPath, EmailType.Career);

                var path = Path.Combine(_environment.WebRootPath, "templates");
                var template = Path.Combine(path, "career-notification.html");

                var subject = $"Job Application - {model.FullName}";
                var date = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");

                var builder = new StringBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(template))
                {
                    builder.Append(SourceReader.ReadToEnd());
                }

                string messageBody = string.Format(builder.ToString(),
                    subject,
                    model.FullName,
                    model.Email,
                    model.Phone,
                    date
                    );

                EmailRequest emailRequest = new()
                {
                    RecieverEmailAddress = model.Email,
                    Subject = subject,
                    Body = messageBody
                };

                await _emailSender.SendEmailAsync(model.OrganizationId, emailRequest);

                ViewBag.Success = "Your application has been forwarded.";

                return RedirectToAction(nameof(Apply));
            }
            catch (DbUpdateConcurrencyException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");

                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            return View(model);
        }

        public async Task<IActionResult> ListApplications()
        {
            return View(await _context.Applicants.Include(x => x.Vacancy).ToListAsync());
        }

        public async Task<IActionResult> ListApplications(string searchString) //  encrpt string
        {
            var applicants = from app in _context.Applicants
                             select app;
            if (!string.IsNullOrEmpty(searchString) )
            {
                applicants = applicants.Where(s => s.FirstName.Contains(searchString) || s.Email.Contains(searchString));
            }

            return View(await applicants.ToListAsync());
        }

        public async Task<IActionResult> ViewApplication(string id)
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

            var applicant = await _context.Applicants
                .FirstOrDefaultAsync(x => x.ApplicantId == num);
            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        public async Task<IActionResult> Download(string id, string fileType = null)
        {
            if (id == null)
                return Content("file name not present");
            if (fileType == null) { return Content("file type not known"); }

            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            var fileUpload = await _context.Applicants
                .FirstOrDefaultAsync(m => m.ApplicantId == num);
            if (fileUpload == null)
            {
                return NotFound();
            }

            var filename = string.Empty;

            if (fileType.Equals("Resume"))
            {
                filename = fileUpload.ResumePath;
            }
            else { filename = fileUpload.CoverLetterPath; }
            
            var path = Path.Combine(
                               _environment.WebRootPath, "documents", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory, _fileService.GetContentType(path), Path.GetFileName(path));
        }

        public async Task<IActionResult> ListInterviews()
        {
            return View(await _context.Interviews.Include(x => x.Applicant).ToListAsync());
        }

        public async Task<IActionResult> AddInterview()
        {
            var user = await _userManager.GetUserAsync(User);

            InterviewViewModel interview = new() { InterviewDate = DateTime.Today };

            if (user.OrganizationId != null)
            {
                interview.OrganizationId = (int)user.OrganizationId;
            }

            ViewData["ApplicantId"] = new SelectList(_context.Interviews.Where(x => x.OrganizationId == (int)user.OrganizationId), "ApplicantId", "FullName");
            ViewData["EmployeeId"] = new SelectList(_context.Employees.Where(x => x.OrganizationId == (int)user.OrganizationId), "EmployeeId", "FullName");

            return View(interview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInterview([Bind("InterviewId,ApplicationId,InterviewDate,InterviewTime,Note,InterviewersLists")] InterviewViewModel interviewView)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                interviewView.OrganizationId = (int)user.OrganizationId;
            }

            Interview interview = new()
            {
                ApplicantId = interviewView.ApplicantId,
                InterviewDate = interviewView.InterviewDate,
                InterviewTime = interviewView.InterviewDate,
                Note = interviewView.Note,
                AddedBy = user.UserName
            };

            if (ModelState.IsValid)
            {
                await _context.AddAsync(interview);
                await _context.SaveChangesAsync();

                foreach(var emp in interviewView.InterviewersLists)
                {
                    Interviewer interviewer = new()
                    {
                        InterviewId = interview.InterviewId,
                        EmployeeId = emp.EmployeeId,
                        DateAdded = DateTime.Now
                    };

                    await _context.AddAsync(interviewer);
                    await _context.SaveChangesAsync();

                    //ToDo: implement sent notification to interviewer
                }

                var newValue = JsonConvert.SerializeObject(interview);
                
                await _audit.UpdateAuditTrail((int)user.OrganizationId, interview.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                return RedirectToAction(nameof(ListInterviews));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["ApplicantId"] = new SelectList(_context.Interviews.Where(x => x.OrganizationId == (int)user.OrganizationId), "ApplicantId", "FullName", interview.ApplicantId);

            return View(interview);
        }

        public async Task<IActionResult> EditInterview(string id)
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

            var interview = await _context.Interviews.Include(m => m.Applicant).FirstOrDefaultAsync(x => x.InterviewId == num);
            if (interview == null)
            {
                return NotFound();
            }

            InterviewViewModel interviewView = new()
            {
                InterviewId = interview.InterviewId,
                ApplicantId = interview.ApplicantId,
                InterviewDate = interview.InterviewDate,
                InterviewTime = interview.InterviewDate,
                Note = interview.Note,
                Interviewers = await _context.Interviewers.Where(x => x.InterviewId == interview.InterviewId).ToListAsync(),
                InterviewersLists = new List<InterviewersList>()
            };

            foreach(var interviewer in interviewView.Interviewers)
            {
                Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == interviewer.EmployeeId);
                InterviewersList interviewersList = new()
                {
                    EmployeeId = interviewer.EmployeeId,
                    FullName = employee.FullName
                };

                interviewView.InterviewersLists.Add(interviewersList);
            }

            ViewData["ApplicantId"] = new SelectList(_context.Interviews.Where(x => x.OrganizationId == (int)user.OrganizationId), "ApplicantId", "FullName", interview.ApplicantId);

            return View(interview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInterview(string id, [Bind("InterviewId,ApplicationId,InterviewDate,InterviewTime,Note,InterviewersLists")] InterviewViewModel interviewView)
        {
            int num = Resolver(id);
            if (num == 0)
            {
                return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
            }

            if (num != interviewView.InterviewId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            Interview interview = new()
            {
                InterviewId = interviewView.InterviewId,
                ApplicantId = interviewView.ApplicantId,
                InterviewDate = interviewView.InterviewDate,
                InterviewTime = interviewView.InterviewDate,
                Note = interviewView.Note,
                UpdatedBy = user.UserName,
                DateUpdated = DateTime.Now
            };

            var oldValue = JsonConvert.SerializeObject(await _context.Vacancies.FirstOrDefaultAsync(x => x.VacancyId == interview.InterviewId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ChangeTracker.Clear();
                    _context.Update(interview);
                    await _context.SaveChangesAsync();

                    List<Interviewer> interviewers = await _context.Interviewers.Where(x => x.InterviewId == interview.InterviewId).ToListAsync();

                    foreach(var emp in interviewView.InterviewersLists)
                    {
                        if (!interviewers.Any(x => x.EmployeeId == emp.EmployeeId))
                        {
                            Interviewer interviewer = new()
                            {
                                InterviewId = interview.InterviewId,
                                EmployeeId = emp.EmployeeId,
                                DateAdded = DateTime.Now
                            };

                            await _context.AddAsync(interviewer);
                            await _context.SaveChangesAsync();

                            //ToDo: implement sent notification to interviewer
                        }
                    }

                    foreach(var interviewer in interviewers)
                    {
                        if (!interviewView.InterviewersLists.Any(x => x.EmployeeId == interviewer.EmployeeId))
                        {
                            _context.Remove(interviewer);
                            await _context.SaveChangesAsync();

                            //ToDo: implement sent notification to interviewer -- removal
                        }
                    }

                    var newValue = JsonConvert.SerializeObject(interview);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, interview.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);

                    return RedirectToAction(nameof(ListInterviews));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!InterviewExists(interview.InterviewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        ViewBag.Message = "Unable to save changes. " +
                            "Try again. And if the problem persists, " +
                            "contact your system administrator.";
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again. And if the problem persists, " +
                    "contact your system administrator.";
            }

            ViewData["ApplicantId"] = new SelectList(_context.Interviews.Where(x => x.OrganizationId == (int)user.OrganizationId), "ApplicantId", "FullName", interview.ApplicantId);

            return View(interviewView);
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

            Client client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == num);
            client.Notes += stringBuilder.ToString();

            _context.Update(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EditInterview), new { id = _protector.Encode(num.ToString()) });

        }

        public static bool ReCaptchaPassed(string gRecaptchaResponse, string secret, ILogger logger)
        {
            HttpClient httpClient = new();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                logger.LogError("Error while sending request to ReCaptcha");
                return false;
            }

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                return false;
            }

            return true;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(x => x.EmployeeId == id);
        }

        public bool InterviewExists(int id)
        {
            return _context.Interviews.Any(x => x.InterviewId == id);
        }

        public bool InterviewerExists(int id)
        {
            return _context.Interviewers.Any(x => x.InterviewerId == id);   
        }

        public bool VacancyExists(int id)
        {
            return _context.Vacancies.Any(x => x.VacancyId == id);
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

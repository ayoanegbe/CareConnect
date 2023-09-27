using CareConnect.Data;
using CareConnect.Enums;
using CareConnect.Interfaces;
using CareConnect.Models;
using CareConnect.Models.CareConnectViewModels;
using CareConnect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

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
        private readonly EmailSettings _emailSettings;

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
        public async Task<IActionResult> Index(int? id) // find a way to pass organization ID
        {
            return View(
                await _context.Employees.Where(x => x.OrganizationId == id)
                .Include(d => d.Department)
                .Include(j => j.JobTitle)
                .ToListAsync());
        }

        public IActionResult AddEmployee(int? id) // find a way to pass organization ID
        {
            Employee employee = new();

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == id), "DepartmentId", "Name");
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == id), "JobTitleId", "Title");

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee([Bind("EmployeeId,DepartmentId,JobTitleId,FirstName,MiddleName,LastName,Gender,BirthDate,Address,City,PostCode,Phone,Email,EmergencyContactPhone,EmergencyContactAddress,Relationship,DateJoined,Status,ContractorType,ImmigrationStatus,SocialInsuranceNumber,LineManagerId,PaymentMethod,BankName,BankCode,TransitCode,AccountNumber,PaymentFrequency")] Employee employee)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                employee.OrganizationId = (int)user.OrganizationId;
            }

            employee.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(employee);
                await _context.SaveChangesAsync();

                var newValue = JsonConvert.SerializeObject(employee);

                await _audit.UpdateAuditTrail((int)user.OrganizationId, employee.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", employee.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", employee.JobTitleId);

            return View(employee);
        }

        public async Task<IActionResult> EditEmployee(string id)
        {
            var user = await _userManager.GetUserAsync(User);

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

            var employee = await _context.Employees.Include(m => m.Department).Include(t => t.JobTitle).FirstOrDefaultAsync(x => x.EmployeeId == decodedNumber);
            if (employee == null)
            {
                return NotFound();
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", employee.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", employee.JobTitleId);

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(string id, [Bind("EmployeeId,DepartmentId,JobTitleId,FirstName,MiddleName,LastName,Gender,BirthDate,Address,City,PostCode,Phone,Email,EmergencyContactPhone,EmergencyContactAddress,Relationship,DateJoined,Status,ContractorType,ImmigrationStatus,SocialInsuranceNumber,LineManagerId,PaymentMethod,BankName,BankCode,TransitCode,AccountNumber,PaymentFrequency")] Employee employee)
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

            if (decodedNumber != employee.EmployeeId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            employee.UpdatedBy = user.UserName;
            employee.DateUpdated = DateTime.UtcNow;

            var oldValue = JsonConvert.SerializeObject(await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employee.EmployeeId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(employee);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, employee.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);
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
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", employee.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", employee.JobTitleId);

            return View(employee);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ListVacancies()
        {
            return View(await _context.Vacancies.Include(d => d.Department).Include(t => t.JobTitle).ToListAsync());
        }
        
        public IActionResult AddVancancy(int id) // find a way to pass organization ID
        {
            Vacancy vacancy = new();

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == id), "DepartmentId", "Name");
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == id), "JobTitleId", "Title");

            return View(vacancy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVacancy([Bind("VacancyId,JobTitleId,DepartmentId.JobDescription,Requirements,Status")] Vacancy vacancy)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                vacancy.OrganizationId = (int)user.OrganizationId;
            }

            vacancy.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(vacancy);
                await _context.SaveChangesAsync();

                var newValue = JsonConvert.SerializeObject(vacancy);

                await _audit.UpdateAuditTrail((int)user.OrganizationId, vacancy.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", vacancy.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", vacancy.JobTitleId);

            return View(vacancy);
        }

        public async Task<IActionResult> EditVacancy(string id)
        {
            var user = await _userManager.GetUserAsync(User);

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

            var vacancy = await _context.Vacancies.Include(m => m.Department).Include(t => t.JobTitle).FirstOrDefaultAsync(x => x.VacancyId == decodedNumber);
            if (vacancy == null)
            {
                return NotFound();
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", vacancy.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", vacancy.JobTitleId);

            return View(vacancy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVacancy(string id, [Bind("VacancyId,JobTitleId,DepartmentId.JobDescription,Requirements,Status")] Vacancy vacancy)
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

            if (decodedNumber != vacancy.VacancyId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            vacancy.UpdatedBy = user.UserName;
            vacancy.DateUpdated = DateTime.UtcNow;

            var oldValue = JsonConvert.SerializeObject(await _context.Vacancies.FirstOrDefaultAsync(x => x.VacancyId == vacancy.VacancyId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacancy);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(vacancy);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, vacancy.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);
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
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.";
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments.Where(x => x.OrganizationId == (int)user.OrganizationId), "DepartmentId", "Name", vacancy.DepartmentId);
            ViewData["JobTitleId"] = new SelectList(_context.JobTitles.Where(x => x.OrganizationId == (int)user.OrganizationId), "JobTitleId", "Title", vacancy.JobTitleId);

            return View(vacancy);
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

        public async Task<IActionResult> ListApplications(string searchString)
        {
            var applicants = from app in _context.Applicants
                             select app;
            if (!string.IsNullOrEmpty(searchString) )
            {
                applicants = applicants.Where(s => s.FirstName.Contains(searchString) || s.Email.Contains(searchString));
            }

            return View(await applicants.ToListAsync());
        }

        public async Task<IActionResult> ViewApplication(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicants
                .FirstOrDefaultAsync(x => x.ApplicantId == id);
            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        public async Task<IActionResult> Download(int? id, string fileType = null)
        {
            if (id == null)
                return Content("file name not present");
            if (fileType == null) { return Content("file type not known"); }

            var fileUpload = await _context.Applicants
                .FirstOrDefaultAsync(m => m.ApplicantId == id);
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

        public IActionResult AddInterview(int id)
        {
            Interview interview = new();

            ViewData["ApplicantId"] = new SelectList(_context.Interviews.Where(x => x.OrganizationId == id), "ApplicantId", "FullName");

            return View(interview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInterview([Bind("InterviewId,ApplicationId,InterviewDate,InterviewTime,Note")] Interview interview)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.OrganizationId != null)
            {
                interview.OrganizationId = (int)user.OrganizationId;
            }

            interview.AddedBy = user.UserName;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(interview);
                await _context.SaveChangesAsync();

                var newValue = JsonConvert.SerializeObject(interview);
                
                await _audit.UpdateAuditTrail((int)user.OrganizationId, interview.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                return RedirectToAction(nameof(Index));
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

            var interview = await _context.Interviews.Include(m => m.Applicant).FirstOrDefaultAsync(x => x.InterviewId == decodedNumber);
            if (interview == null)
            {
                return NotFound();
            }

            ViewData["ApplicantId"] = new SelectList(_context.Interviews.Where(x => x.OrganizationId == (int)user.OrganizationId), "ApplicantId", "FullName", interview.ApplicantId);

            return View(interview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInterview(string id, [Bind("InterviewId,ApplicationId,InterviewDate,InterviewTime,Note")] Interview interview)
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

            if (decodedNumber != interview.InterviewId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            interview.UpdatedBy = user.UserName;
            interview.DateUpdated = DateTime.UtcNow;

            var oldValue = JsonConvert.SerializeObject(await _context.Vacancies.FirstOrDefaultAsync(x => x.VacancyId == interview.InterviewId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interview);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(interview);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, interview.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);
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
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again. And if the problem persists, " +
                    "contact your system administrator.";
            }

            ViewData["ApplicantId"] = new SelectList(_context.Interviews.Where(x => x.OrganizationId == (int)user.OrganizationId), "ApplicantId", "FullName", interview.ApplicantId);

            return View(interview);
        }

        public IActionResult AddInterviewer()
        {
            Interviewer interviewer = new ();

            return View(interviewer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInterviewer([Bind("InterviewerId,InterviewId,EmployeeId")] Interviewer interviewer)
        {
            var user = await _userManager.GetUserAsync(User);

            interviewer.AddedBy = user.UserName;
            interviewer.IsAvailable = true;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(interviewer);
                await _context.SaveChangesAsync();

                var newValue = JsonConvert.SerializeObject(interviewer);

                await _audit.UpdateAuditTrail((int)user.OrganizationId, interviewer.GetType().Name, UpdateAction.Create, newValue, user.UserName);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again. And if the problem persists, " +
                    "see your system administrator.";
            }

            return View(interviewer);
        }

        public async Task<IActionResult> EditInterviewer(string id)
        {
            var user = await _userManager.GetUserAsync(User);

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

            var interviewer = await _context.Interviewers.Include(m => m.Employee).FirstOrDefaultAsync(x => x.InterviewId == decodedNumber);
            if (interviewer == null)
            {
                return NotFound();
            }

            //ViewData["ApplicantId"] = new SelectList(_context.Interviews.Where(x => x.OrganizationId == (int)user.OrganizationId), "ApplicantId", "FullName", interview.ApplicantId);

            return View(interviewer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInterviewer(string id, [Bind("InterviewerId,InterviewId,EmployeeId,IsAvailable")] Interviewer interviewer)
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

            if (decodedNumber != interviewer.InterviewerId)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            interviewer.UpdatedBy = user.UserName;
            interviewer.DateUpdated = DateTime.UtcNow;

            var oldValue = JsonConvert.SerializeObject(await _context.Interviewers.FirstOrDefaultAsync(x => x.InterviewerId == interviewer.InterviewerId));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interviewer);
                    await _context.SaveChangesAsync();

                    var newValue = JsonConvert.SerializeObject(interviewer);

                    await _audit.UpdateAuditTrail((int)user.OrganizationId, interviewer.GetType().Name, UpdateAction.Update, oldValue, newValue, user.UserName);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!InterviewerExists(interviewer.InterviewerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(LogLevel.Error, "An error has occurred fetching item", ex);
                        return RedirectToAction(nameof(ErrorController.Error), new { Controller = "Error", Action = "Error", code = 500 });
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Message = "Unable to save changes. " +
                    "Try again. And if the problem persists, " +
                    "contact your system administrator.";
            }

            //ViewData["ApplicantId"] = new SelectList(_context.Interviews.Where(x => x.OrganizationId == (int)user.OrganizationId), "ApplicantId", "FullName", interview.ApplicantId);

            return View(interviewer);
        }

        public async Task<IActionResult> RemoveInterviewer(string id)
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

            var interviewer = await _context.Interviewers.FirstOrDefaultAsync(x => x.InterviewId == decodedNumber);
            if (interviewer == null)
            {
                return NotFound();
            }

            return View(interviewer);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveInterviewerConfirmed(string id)
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

            var user = await _userManager.GetUserAsync(User);

            var interviewer = await _context.Interviewers.FirstOrDefaultAsync(x => x.InterviewId == decodedNumber);

            var oldValue = JsonConvert.SerializeObject(interviewer);

            _context.Interviewers.Remove(interviewer);
            await _context.SaveChangesAsync();            

            await _audit.UpdateAuditTrail((int)user.OrganizationId, interviewer.GetType().Name, UpdateAction.Delete, oldValue, null, user.UserName);

            return View(interviewer);
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
    }
}

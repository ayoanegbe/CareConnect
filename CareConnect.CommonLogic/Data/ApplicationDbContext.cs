using CareConnect.CommonLogic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareConnect.CommonLogic.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Allowance> Allowances { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ApplicantDocument> ApplicantDocuments { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientBudgetHistory> ClientBudgetHistories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Deduction> Deductions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmergencyClockOut> EmergencyClockOuts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public DbSet<EmployeeAvailability> EmployeeAvailability { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<EmployeeOvertime> EmployeeOvertimes { get; set; }
        public DbSet<EmployeePreference> EmployeePreferences { get; set; }
        public DbSet<ExpenseReport> ExpenseReports { get; set; }
        public DbSet<FireDrill> FireDrills { get; set; }
        public DbSet<HourlyTimeSheet> HourlyTimeSheets { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Interviewer> Interviewers { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<JobTitleHistory> JobTitlesHistories { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OvertimeComment> OvertimeComments { get; set; }
        public DbSet<PayGrade> PayGrades { get; set; }
        public DbSet<PayGradeLevel> PayGradeLevels { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollAllowance> PayrollAllowances { get; set; }
        public DbSet<PayrollDeduction> PayrollDeductions { get; set; }
        public DbSet<PayrollHistory> PayrollHistories { get; set; }
        public DbSet<Respite> Respites { get; set; }
        public DbSet<RespiteAssignment> RespiteAssignments { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftAssigment> ShiftAssigments { get; set; }
        public DbSet<ShiftPattern> ShiftPatterns { get; set; }
        public DbSet<ShiftRun> ShiftRuns { get; set; }
        public DbSet<ShiftSwapRequest> ShiftSwapRequests { get; set; }
        public DbSet<SupportiveRoomate> SupportiveRoomates { get; set; }
        public DbSet<SmtpSetting> SmtpSettings { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionRate> SubscriptionsRates { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VisitedPage> VisitedPages { get; set; }
    }
}

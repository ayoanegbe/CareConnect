using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobPositions_JobPositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_PayGradeLevelDeductions_Employees_EmployeeId",
                table: "PayGradeLevelDeductions");

            migrationBuilder.DropTable(
                name: "JobPositionHistories");

            migrationBuilder.DropTable(
                name: "JobPositions");

            migrationBuilder.DropIndex(
                name: "IX_PayGradeLevelDeductions_EmployeeId",
                table: "PayGradeLevelDeductions");

            migrationBuilder.DropColumn(
                name: "AllDay",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "Friday",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "Monday",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "Saturday",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "Sunday",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "Thursday",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "Tuesday",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "Wednesday",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "PayGradeLevelDeductions");

            migrationBuilder.RenameColumn(
                name: "EmployeedId",
                table: "PayGradeLevelDeductions",
                newName: "PayGradeLevelId");

            migrationBuilder.RenameColumn(
                name: "UpdateedBy",
                table: "Houses",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "JobPositionId",
                table: "Employees",
                newName: "JobTitleId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_JobPositionId",
                table: "Employees",
                newName: "IX_Employees_JobTitleId");

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "SmtpSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CcEmail",
                table: "SmtpSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "SmtpSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "SmtpSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "SmtpSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SmtpSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "ShiftPatterns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "PayGradeLevels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "PayGradeLevels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "PayGradeLevels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PayGradeLevels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PayGradeLevels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "PayGradeLevelDeductions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "PayGradeLevelDeductions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "PayGradeLevelDeductions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PayGradeLevelDeductions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAssigned",
                table: "Organizations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Organizations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LineManagerId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "Alerts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Alerts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    AuditTrailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.AuditTrailId);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ClientBudgetHistories",
                columns: table => new
                {
                    ClientBudgetHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    BudgetStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BudgetEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientBudgetHistories", x => x.ClientBudgetHistoryId);
                    table.ForeignKey(
                        name: "FK_ClientBudgetHistories_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientBudgetHistories_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_Departments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyClockOuts",
                columns: table => new
                {
                    EmergencyClockOutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeAttendanceId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseManagerId = table.Column<int>(type: "int", nullable: false),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyClockOuts", x => x.EmergencyClockOutId);
                    table.ForeignKey(
                        name: "FK_EmergencyClockOuts_CaseManagers_CaseManagerId",
                        column: x => x.CaseManagerId,
                        principalTable: "CaseManagers",
                        principalColumn: "CaseManagerId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmergencyClockOuts_EmployeeAttendances_EmployeeAttendanceId",
                        column: x => x.EmployeeAttendanceId,
                        principalTable: "EmployeeAttendances",
                        principalColumn: "EmployeeAttendanceId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    JobTitleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.JobTitleId);
                    table.ForeignKey(
                        name: "FK_JobTitles_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionsRates",
                columns: table => new
                {
                    SubscriptionRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    NumberOfEmployees = table.Column<int>(type: "int", nullable: true),
                    NumberOfClients = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionsRates", x => x.SubscriptionRateId);
                    table.ForeignKey(
                        name: "FK_SubscriptionsRates_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "JobTitlesHistories",
                columns: table => new
                {
                    JobTitleHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitleId = table.Column<int>(type: "int", nullable: false),
                    EmployerId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitlesHistories", x => x.JobTitleHistoryId);
                    table.ForeignKey(
                        name: "FK_JobTitlesHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_JobTitlesHistories_JobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitles",
                        principalColumn: "JobTitleId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Vacancies",
                columns: table => new
                {
                    VacancyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    JobTitleId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requirements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancies", x => x.VacancyId);
                    table.ForeignKey(
                        name: "FK_Vacancies_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Vacancies_JobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitles",
                        principalColumn: "JobTitleId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Vacancies_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionRateId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SubscriptionsRates_SubscriptionRateId",
                        column: x => x.SubscriptionRateId,
                        principalTable: "SubscriptionsRates",
                        principalColumn: "SubscriptionRateId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    ApplicantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VacancyId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverLetterPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateApplied = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.ApplicantId);
                    table.ForeignKey(
                        name: "FK_Applicants_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Applicants_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "VacancyId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantDocuments",
                columns: table => new
                {
                    ApplicantDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantDocuments", x => x.ApplicantDocumentId);
                    table.ForeignKey(
                        name: "FK_ApplicantDocuments_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "ApplicantId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Interviews",
                columns: table => new
                {
                    InterviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    InterviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterviewTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.InterviewId);
                    table.ForeignKey(
                        name: "FK_Interviews_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "ApplicantId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Interviews_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Interviewers",
                columns: table => new
                {
                    InterviewerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterviewId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviewers", x => x.InterviewerId);
                    table.ForeignKey(
                        name: "FK_Interviewers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Interviewers_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalTable: "Interviews",
                        principalColumn: "InterviewId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmtpSettings_OrganizationId",
                table: "SmtpSettings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftPatterns_OrganizationId",
                table: "ShiftPatterns",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayGradeLevelDeductions_PayGradeLevelId",
                table: "PayGradeLevelDeductions",
                column: "PayGradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_OrganizationId",
                table: "Alerts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantDocuments_ApplicantId",
                table: "ApplicantDocuments",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_OrganizationId",
                table: "Applicants",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_VacancyId",
                table: "Applicants",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_OrganizationId",
                table: "AuditTrails",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientBudgetHistories_ClientId",
                table: "ClientBudgetHistories",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientBudgetHistories_CurrencyId",
                table: "ClientBudgetHistories",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationId",
                table: "Departments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyClockOuts_CaseManagerId",
                table: "EmergencyClockOuts",
                column: "CaseManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyClockOuts_EmployeeAttendanceId",
                table: "EmergencyClockOuts",
                column: "EmployeeAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Interviewers_EmployeeId",
                table: "Interviewers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Interviewers_InterviewId",
                table: "Interviewers",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_ApplicantId",
                table: "Interviews",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_OrganizationId",
                table: "Interviews",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_OrganizationId",
                table: "JobTitles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitlesHistories_EmployeeId",
                table: "JobTitlesHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitlesHistories_JobTitleId",
                table: "JobTitlesHistories",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_OrganizationId",
                table: "Subscriptions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriptionRateId",
                table: "Subscriptions",
                column: "SubscriptionRateId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionsRates_CurrencyId",
                table: "SubscriptionsRates",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_DepartmentId",
                table: "Vacancies",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_JobTitleId",
                table: "Vacancies",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_OrganizationId",
                table: "Vacancies",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Organizations_OrganizationId",
                table: "Alerts",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobTitles_JobTitleId",
                table: "Employees",
                column: "JobTitleId",
                principalTable: "JobTitles",
                principalColumn: "JobTitleId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PayGradeLevelDeductions_PayGradeLevels_PayGradeLevelId",
                table: "PayGradeLevelDeductions",
                column: "PayGradeLevelId",
                principalTable: "PayGradeLevels",
                principalColumn: "PayGradeLevelId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftPatterns_Organizations_OrganizationId",
                table: "ShiftPatterns",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SmtpSettings_Organizations_OrganizationId",
                table: "SmtpSettings",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Organizations_OrganizationId",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobTitles_JobTitleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_PayGradeLevelDeductions_PayGradeLevels_PayGradeLevelId",
                table: "PayGradeLevelDeductions");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftPatterns_Organizations_OrganizationId",
                table: "ShiftPatterns");

            migrationBuilder.DropForeignKey(
                name: "FK_SmtpSettings_Organizations_OrganizationId",
                table: "SmtpSettings");

            migrationBuilder.DropTable(
                name: "ApplicantDocuments");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "ClientBudgetHistories");

            migrationBuilder.DropTable(
                name: "EmergencyClockOuts");

            migrationBuilder.DropTable(
                name: "Interviewers");

            migrationBuilder.DropTable(
                name: "JobTitlesHistories");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Interviews");

            migrationBuilder.DropTable(
                name: "SubscriptionsRates");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "Vacancies");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "JobTitles");

            migrationBuilder.DropIndex(
                name: "IX_SmtpSettings_OrganizationId",
                table: "SmtpSettings");

            migrationBuilder.DropIndex(
                name: "IX_ShiftPatterns_OrganizationId",
                table: "ShiftPatterns");

            migrationBuilder.DropIndex(
                name: "IX_PayGradeLevelDeductions_PayGradeLevelId",
                table: "PayGradeLevelDeductions");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_OrganizationId",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "SmtpSettings");

            migrationBuilder.DropColumn(
                name: "CcEmail",
                table: "SmtpSettings");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "SmtpSettings");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "SmtpSettings");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "SmtpSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SmtpSettings");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "ShiftPatterns");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "PayGradeLevels");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "PayGradeLevels");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "PayGradeLevels");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PayGradeLevels");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PayGradeLevels");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "PayGradeLevelDeductions");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "PayGradeLevelDeductions");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "PayGradeLevelDeductions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PayGradeLevelDeductions");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "DateAssigned",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LineManagerId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Alerts");

            migrationBuilder.RenameColumn(
                name: "PayGradeLevelId",
                table: "PayGradeLevelDeductions",
                newName: "EmployeedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Houses",
                newName: "UpdateedBy");

            migrationBuilder.RenameColumn(
                name: "JobTitleId",
                table: "Employees",
                newName: "JobPositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_JobTitleId",
                table: "Employees",
                newName: "IX_Employees_JobPositionId");

            migrationBuilder.AddColumn<bool>(
                name: "AllDay",
                table: "ShiftPatterns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Friday",
                table: "ShiftPatterns",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Monday",
                table: "ShiftPatterns",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Saturday",
                table: "ShiftPatterns",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sunday",
                table: "ShiftPatterns",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Thursday",
                table: "ShiftPatterns",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Tuesday",
                table: "ShiftPatterns",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Wednesday",
                table: "ShiftPatterns",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "PayGradeLevelDeductions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobPositionHistories",
                columns: table => new
                {
                    JobPositionHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    EmployerId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositionHistories", x => x.JobPositionHistoryId);
                    table.ForeignKey(
                        name: "FK_JobPositionHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "JobPositions",
                columns: table => new
                {
                    JobPositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.JobPositionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayGradeLevelDeductions_EmployeeId",
                table: "PayGradeLevelDeductions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPositionHistories_EmployeeId",
                table: "JobPositionHistories",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobPositions_JobPositionId",
                table: "Employees",
                column: "JobPositionId",
                principalTable: "JobPositions",
                principalColumn: "JobPositionId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PayGradeLevelDeductions_Employees_EmployeeId",
                table: "PayGradeLevelDeductions",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }
    }
}

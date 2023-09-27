using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Companies_CompanyId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Companies_CompanyId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Houses_HouseId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Houses_CompanyId",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "DateAssigned",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "DateDeAssigned",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "ShiftDate",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsStaff",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Shifts",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "NumbersNeeded",
                table: "Shifts",
                newName: "ShiftPatternId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Shifts",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_CompanyId",
                table: "Shifts",
                newName: "IX_Shifts_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "ShiftAssigments",
                newName: "DeAssignedBy");

            migrationBuilder.RenameColumn(
                name: "AssignedTo",
                table: "ShiftAssigments",
                newName: "AssignedBy");

            migrationBuilder.RenameColumn(
                name: "AssignedDate",
                table: "ShiftAssigments",
                newName: "StartDate");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "HouseId",
                table: "Shifts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Shifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumbersAssigned",
                table: "Shifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumbersRequired",
                table: "Shifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Perpetual",
                table: "Shifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAssigned",
                table: "ShiftAssigments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeassigned",
                table: "ShiftAssigments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeAssign",
                table: "ShiftAssigments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "ShiftAssigments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ShiftAssigments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Houses",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Houses",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Houses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdateedBy",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbols = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAttendances",
                columns: table => new
                {
                    EmployeeAttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftAssigmentId = table.Column<int>(type: "int", nullable: false),
                    ClockIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClockOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoursWorked = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAttendances", x => x.EmployeeAttendanceId);
                    table.ForeignKey(
                        name: "FK_EmployeeAttendances_ShiftAssigments_ShiftAssigmentId",
                        column: x => x.ShiftAssigmentId,
                        principalTable: "ShiftAssigments",
                        principalColumn: "ShiftAssigmentId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "JobPositions",
                columns: table => new
                {
                    JobPositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.JobPositionId);
                });

            migrationBuilder.CreateTable(
                name: "ShiftPatterns",
                columns: table => new
                {
                    ShiftPatternId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatternName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatternDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Monday = table.Column<bool>(type: "bit", nullable: true),
                    Tuesday = table.Column<bool>(type: "bit", nullable: true),
                    Wednesday = table.Column<bool>(type: "bit", nullable: true),
                    Thursday = table.Column<bool>(type: "bit", nullable: true),
                    Friday = table.Column<bool>(type: "bit", nullable: true),
                    Saturday = table.Column<bool>(type: "bit", nullable: true),
                    Sunday = table.Column<bool>(type: "bit", nullable: true),
                    AllDay = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftPatterns", x => x.ShiftPatternId);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                    table.ForeignKey(
                        name: "FK_Organizations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPersonPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ContractorType = table.Column<int>(type: "int", nullable: true),
                    ImmigrationStatus = table.Column<int>(type: "int", nullable: false),
                    SocialInsuranceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobPositionId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransitCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentFrequency = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_JobPositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalTable: "JobPositions",
                        principalColumn: "JobPositionId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Employees_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PayGrades",
                columns: table => new
                {
                    PayGradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayGrades", x => x.PayGradeId);
                    table.ForeignKey(
                        name: "FK_PayGrades_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransitCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorId);
                    table.ForeignKey(
                        name: "FK_Vendors_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    HouseId = table.Column<int>(type: "int", nullable: true),
                    ResidentialType = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuadianPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyPhysician = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Psychiatrist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Budget = table.Column<double>(type: "float", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    BudgetStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BudgetEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Clients_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Clients_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Clients_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "HouseId");
                    table.ForeignKey(
                        name: "FK_Clients_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CaseManagers",
                columns: table => new
                {
                    CaseManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseManagers", x => x.CaseManagerId);
                    table.ForeignKey(
                        name: "FK_CaseManagers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CaseManagers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAvailability",
                columns: table => new
                {
                    AvailabilityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DateAvailable = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAvailability", x => x.AvailabilityId);
                    table.ForeignKey(
                        name: "FK_EmployeeAvailability_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDocuments",
                columns: table => new
                {
                    EmployeeDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDocuments", x => x.EmployeeDocumentId);
                    table.ForeignKey(
                        name: "FK_EmployeeDocuments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeOvertimes",
                columns: table => new
                {
                    EmployeeOvertimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OvertimeHoursWorked = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOvertimes", x => x.EmployeeOvertimeId);
                    table.ForeignKey(
                        name: "FK_EmployeeOvertimes_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePreferences",
                columns: table => new
                {
                    PreferenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ShiftPatternId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePreferences", x => x.PreferenceId);
                    table.ForeignKey(
                        name: "FK_EmployeePreferences_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmployeePreferences_ShiftPatterns_ShiftPatternId",
                        column: x => x.ShiftPatternId,
                        principalTable: "ShiftPatterns",
                        principalColumn: "ShiftPatternId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "JobPositionHistories",
                columns: table => new
                {
                    JobPositionHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployerId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "PayGradeLevelDeductions",
                columns: table => new
                {
                    PayGradeLevelDeductionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeedId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    TaxRate = table.Column<double>(type: "float", nullable: false),
                    HealthInsurance = table.Column<double>(type: "float", nullable: false),
                    RetirementContribution = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayGradeLevelDeductions", x => x.PayGradeLevelDeductionId);
                    table.ForeignKey(
                        name: "FK_PayGradeLevelDeductions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "PayrollHistories",
                columns: table => new
                {
                    PayrollHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    PayPeriodStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayPeriodEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegularHoursWorked = table.Column<double>(type: "float", nullable: false),
                    OvertimeHoursWorked = table.Column<double>(type: "float", nullable: false),
                    GrossPay = table.Column<double>(type: "float", nullable: false),
                    TotalDeduction = table.Column<double>(type: "float", nullable: true),
                    NetPay = table.Column<double>(type: "float", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollHistories", x => x.PayrollHistoryId);
                    table.ForeignKey(
                        name: "FK_PayrollHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PayrollHistories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ShiftSwapRequests",
                columns: table => new
                {
                    SwapRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalShiftId = table.Column<int>(type: "int", nullable: false),
                    RequestedShiftId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DateRequested = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftSwapRequests", x => x.SwapRequestId);
                    table.ForeignKey(
                        name: "FK_ShiftSwapRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ShiftSwapRequests_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "ShiftId");
                });

            migrationBuilder.CreateTable(
                name: "PayGradeLevels",
                columns: table => new
                {
                    PayGradeLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayGradeId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    BasicSalary = table.Column<double>(type: "float", nullable: false),
                    HourlyRate = table.Column<double>(type: "float", nullable: false),
                    OvertimeRate = table.Column<double>(type: "float", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayGradeLevels", x => x.PayGradeLevelId);
                    table.ForeignKey(
                        name: "FK_PayGradeLevels_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PayGradeLevels_PayGrades_PayGradeId",
                        column: x => x.PayGradeId,
                        principalTable: "PayGrades",
                        principalColumn: "PayGradeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseReports",
                columns: table => new
                {
                    ExpenseReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CashIn = table.Column<double>(type: "float", nullable: true),
                    CashOut = table.Column<double>(type: "float", nullable: true),
                    Balance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseReports", x => x.ExpenseReportId);
                    table.ForeignKey(
                        name: "FK_ExpenseReports_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseReports_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseReports_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "HourlyTimeSheets",
                columns: table => new
                {
                    HourlyTimeSheetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Hours = table.Column<int>(type: "int", nullable: true),
                    Kilometer = table.Column<double>(type: "float", nullable: true),
                    Activities = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourlyTimeSheets", x => x.HourlyTimeSheetId);
                    table.ForeignKey(
                        name: "FK_HourlyTimeSheets_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HourlyTimeSheets_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HourlyTimeSheets_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Respites",
                columns: table => new
                {
                    RespiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ShiftPatternId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Perpetual = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAssigned = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    RespiteType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respites", x => x.RespiteId);
                    table.ForeignKey(
                        name: "FK_Respites_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Respites_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Respites_ShiftPatterns_ShiftPatternId",
                        column: x => x.ShiftPatternId,
                        principalTable: "ShiftPatterns",
                        principalColumn: "ShiftPatternId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SupportiveRoomates",
                columns: table => new
                {
                    SupportiveRoomateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportiveRoomates", x => x.SupportiveRoomateId);
                    table.ForeignKey(
                        name: "FK_SupportiveRoomates_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SupportiveRoomates_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FireDrills",
                columns: table => new
                {
                    FireDrillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CaseManagerId = table.Column<int>(type: "int", nullable: false),
                    DrillDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DrillTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeToEvacuate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DetectorActivated = table.Column<bool>(type: "bit", nullable: false),
                    FireLocationIndicated = table.Column<bool>(type: "bit", nullable: false),
                    ExitPointsIndicated = table.Column<bool>(type: "bit", nullable: false),
                    Contacted911 = table.Column<bool>(type: "bit", nullable: false),
                    WindowsAndDoorsClosed = table.Column<bool>(type: "bit", nullable: false),
                    PersonalInformationTaken = table.Column<bool>(type: "bit", nullable: false),
                    EveryoneGetOutSafely = table.Column<bool>(type: "bit", nullable: false),
                    HeadCountTaken = table.Column<bool>(type: "bit", nullable: false),
                    MeetingPoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NaturalSupports = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlarmsCheckDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FireExtinguisherMaintenance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EvacuationPlanPosted = table.Column<bool>(type: "bit", nullable: false),
                    Where = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireDrills", x => x.FireDrillId);
                    table.ForeignKey(
                        name: "FK_FireDrills_CaseManagers_CaseManagerId",
                        column: x => x.CaseManagerId,
                        principalTable: "CaseManagers",
                        principalColumn: "CaseManagerId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FireDrills_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FireDrills_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FireDrills_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OvertimeComments",
                columns: table => new
                {
                    OvertimeCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeOvertimeId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeComments", x => x.OvertimeCommentId);
                    table.ForeignKey(
                        name: "FK_OvertimeComments_EmployeeOvertimes_EmployeeOvertimeId",
                        column: x => x.EmployeeOvertimeId,
                        principalTable: "EmployeeOvertimes",
                        principalColumn: "EmployeeOvertimeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RespiteAssignments",
                columns: table => new
                {
                    RespiteAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RespiteId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false),
                    DateDeclined = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeAssign = table.Column<bool>(type: "bit", nullable: false),
                    DateDeassigned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeAssignedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespiteAssignments", x => x.RespiteAssignmentId);
                    table.ForeignKey(
                        name: "FK_RespiteAssignments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_RespiteAssignments_Respites_RespiteId",
                        column: x => x.RespiteId,
                        principalTable: "Respites",
                        principalColumn: "RespiteId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ClientId",
                table: "Shifts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ShiftPatternId",
                table: "Shifts",
                column: "ShiftPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssigments_EmployeeId",
                table: "ShiftAssigments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_CompanyId",
                table: "Houses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_OrganizationId",
                table: "Houses",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseManagers_EmployeeId",
                table: "CaseManagers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseManagers_OrganizationId",
                table: "CaseManagers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CurrencyId",
                table: "Clients",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CustomerId",
                table: "Clients",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_HouseId",
                table: "Clients",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_OrganizationId",
                table: "Clients",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_OrganizationId",
                table: "Customers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_ShiftAssigmentId",
                table: "EmployeeAttendances",
                column: "ShiftAssigmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAvailability_EmployeeId",
                table: "EmployeeAvailability",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_EmployeeId",
                table: "EmployeeDocuments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOvertimes_EmployeeId",
                table: "EmployeeOvertimes",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePreferences_EmployeeId",
                table: "EmployeePreferences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePreferences_ShiftPatternId",
                table: "EmployeePreferences",
                column: "ShiftPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobPositionId",
                table: "Employees",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OrganizationId",
                table: "Employees",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReports_ClientId",
                table: "ExpenseReports",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReports_EmployeeId",
                table: "ExpenseReports",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReports_OrganizationId",
                table: "ExpenseReports",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_FireDrills_CaseManagerId",
                table: "FireDrills",
                column: "CaseManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_FireDrills_ClientId",
                table: "FireDrills",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_FireDrills_EmployeeId",
                table: "FireDrills",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FireDrills_OrganizationId",
                table: "FireDrills",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_HourlyTimeSheets_ClientId",
                table: "HourlyTimeSheets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_HourlyTimeSheets_EmployeeId",
                table: "HourlyTimeSheets",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HourlyTimeSheets_OrganizationId",
                table: "HourlyTimeSheets",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPositionHistories_EmployeeId",
                table: "JobPositionHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_TenantId",
                table: "Organizations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OvertimeComments_EmployeeOvertimeId",
                table: "OvertimeComments",
                column: "EmployeeOvertimeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayGradeLevelDeductions_EmployeeId",
                table: "PayGradeLevelDeductions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayGradeLevels_CurrencyId",
                table: "PayGradeLevels",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PayGradeLevels_PayGradeId",
                table: "PayGradeLevels",
                column: "PayGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayGrades_OrganizationId",
                table: "PayGrades",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollHistories_EmployeeId",
                table: "PayrollHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollHistories_OrganizationId",
                table: "PayrollHistories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_RespiteAssignments_EmployeeId",
                table: "RespiteAssignments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RespiteAssignments_RespiteId",
                table: "RespiteAssignments",
                column: "RespiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Respites_ClientId",
                table: "Respites",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Respites_OrganizationId",
                table: "Respites",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Respites_ShiftPatternId",
                table: "Respites",
                column: "ShiftPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSwapRequests_EmployeeId",
                table: "ShiftSwapRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSwapRequests_ShiftId",
                table: "ShiftSwapRequests",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportiveRoomates_ClientId",
                table: "SupportiveRoomates",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportiveRoomates_OrganizationId",
                table: "SupportiveRoomates",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_OrganizationId",
                table: "Vendors",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organizations_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Companies_CompanyId",
                table: "Houses",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Organizations_OrganizationId",
                table: "Houses",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssigments_Employees_EmployeeId",
                table: "ShiftAssigments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Clients_ClientId",
                table: "Shifts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Houses_HouseId",
                table: "Shifts",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Organizations_OrganizationId",
                table: "Shifts",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_ShiftPatterns_ShiftPatternId",
                table: "Shifts",
                column: "ShiftPatternId",
                principalTable: "ShiftPatterns",
                principalColumn: "ShiftPatternId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organizations_OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Companies_CompanyId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Organizations_OrganizationId",
                table: "Houses");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssigments_Employees_EmployeeId",
                table: "ShiftAssigments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Clients_ClientId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Houses_HouseId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Organizations_OrganizationId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_ShiftPatterns_ShiftPatternId",
                table: "Shifts");

            migrationBuilder.DropTable(
                name: "EmployeeAttendances");

            migrationBuilder.DropTable(
                name: "EmployeeAvailability");

            migrationBuilder.DropTable(
                name: "EmployeeDocuments");

            migrationBuilder.DropTable(
                name: "EmployeePreferences");

            migrationBuilder.DropTable(
                name: "ExpenseReports");

            migrationBuilder.DropTable(
                name: "FireDrills");

            migrationBuilder.DropTable(
                name: "HourlyTimeSheets");

            migrationBuilder.DropTable(
                name: "JobPositionHistories");

            migrationBuilder.DropTable(
                name: "OvertimeComments");

            migrationBuilder.DropTable(
                name: "PayGradeLevelDeductions");

            migrationBuilder.DropTable(
                name: "PayGradeLevels");

            migrationBuilder.DropTable(
                name: "PayrollHistories");

            migrationBuilder.DropTable(
                name: "RespiteAssignments");

            migrationBuilder.DropTable(
                name: "ShiftSwapRequests");

            migrationBuilder.DropTable(
                name: "SupportiveRoomates");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "CaseManagers");

            migrationBuilder.DropTable(
                name: "EmployeeOvertimes");

            migrationBuilder.DropTable(
                name: "PayGrades");

            migrationBuilder.DropTable(
                name: "Respites");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "ShiftPatterns");

            migrationBuilder.DropTable(
                name: "JobPositions");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_ClientId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_ShiftPatternId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_ShiftAssigments_EmployeeId",
                table: "ShiftAssigments");

            migrationBuilder.DropIndex(
                name: "IX_Houses_CompanyId",
                table: "Houses");

            migrationBuilder.DropIndex(
                name: "IX_Houses_OrganizationId",
                table: "Houses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "NumbersAssigned",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "NumbersRequired",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Perpetual",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "DateAssigned",
                table: "ShiftAssigments");

            migrationBuilder.DropColumn(
                name: "DateDeassigned",
                table: "ShiftAssigments");

            migrationBuilder.DropColumn(
                name: "DeAssign",
                table: "ShiftAssigments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "ShiftAssigments");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ShiftAssigments");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "UpdateedBy",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Shifts",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "ShiftPatternId",
                table: "Shifts",
                newName: "NumbersNeeded");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Shifts",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_OrganizationId",
                table: "Shifts",
                newName: "IX_Shifts_CompanyId");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "ShiftAssigments",
                newName: "AssignedDate");

            migrationBuilder.RenameColumn(
                name: "DeAssignedBy",
                table: "ShiftAssigments",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "AssignedBy",
                table: "ShiftAssigments",
                newName: "AssignedTo");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HouseId",
                table: "Shifts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAssigned",
                table: "Shifts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeAssigned",
                table: "Shifts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Shifts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ShiftDate",
                table: "Shifts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Houses",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Houses",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Condition",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStaff",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Houses_CompanyId",
                table: "Houses",
                column: "CompanyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Companies_CompanyId",
                table: "Houses",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Companies_CompanyId",
                table: "Shifts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Houses_HouseId",
                table: "Shifts",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "HouseId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}

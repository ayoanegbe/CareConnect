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
                name: "FK_CaseManagers_Employees_EmployeeId",
                table: "CaseManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseManagers_Organizations_OrganizationId",
                table: "CaseManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_EmergencyClockOuts_CaseManagers_CaseManagerId",
                table: "EmergencyClockOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_FireDrills_CaseManagers_CaseManagerId",
                table: "FireDrills");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollHistories_Employees_EmployeeId",
                table: "PayrollHistories");

            migrationBuilder.DropTable(
                name: "PayGradeLevelDeductions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseManagers",
                table: "CaseManagers");

            migrationBuilder.DropColumn(
                name: "GrossPay",
                table: "PayrollHistories");

            migrationBuilder.DropColumn(
                name: "NetPay",
                table: "PayrollHistories");

            migrationBuilder.DropColumn(
                name: "OvertimeHoursWorked",
                table: "PayrollHistories");

            migrationBuilder.DropColumn(
                name: "PayPeriodEndDate",
                table: "PayrollHistories");

            migrationBuilder.DropColumn(
                name: "PayPeriodStartDate",
                table: "PayrollHistories");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "PayrollHistories");

            migrationBuilder.DropColumn(
                name: "RegularHoursWorked",
                table: "PayrollHistories");

            migrationBuilder.DropColumn(
                name: "TotalDeduction",
                table: "PayrollHistories");

            migrationBuilder.RenameTable(
                name: "CaseManagers",
                newName: "CaseManager");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "PayrollHistories",
                newName: "PayrollId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollHistories_EmployeeId",
                table: "PayrollHistories",
                newName: "IX_PayrollHistories_PayrollId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseManagers_OrganizationId",
                table: "CaseManager",
                newName: "IX_CaseManager_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseManagers_EmployeeId",
                table: "CaseManager",
                newName: "IX_CaseManager_EmployeeId");

            migrationBuilder.AlterColumn<string>(
                name: "TransitCode",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BankCode",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "HourlyRate",
                table: "PayGradeLevels",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "PayGradeLevelId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseManager",
                table: "CaseManager",
                column: "CaseManagerId");

            migrationBuilder.CreateTable(
                name: "Allowances",
                columns: table => new
                {
                    AllowanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayGradeLevelId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allowances", x => x.AllowanceId);
                    table.ForeignKey(
                        name: "FK_Allowances_PayGradeLevels_PayGradeLevelId",
                        column: x => x.PayGradeLevelId,
                        principalTable: "PayGradeLevels",
                        principalColumn: "PayGradeLevelId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Deductions",
                columns: table => new
                {
                    DeductionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayGradeLevelId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deductions", x => x.DeductionId);
                    table.ForeignKey(
                        name: "FK_Deductions_PayGradeLevels_PayGradeLevelId",
                        column: x => x.PayGradeLevelId,
                        principalTable: "PayGradeLevels",
                        principalColumn: "PayGradeLevelId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    PayrollId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Payrolls", x => x.PayrollId);
                    table.ForeignKey(
                        name: "FK_Payrolls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Payrolls_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PayrollAllowances",
                columns: table => new
                {
                    PayrollAllowanceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PayrollId = table.Column<int>(type: "int", nullable: false),
                    AllowanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollAllowances", x => x.PayrollAllowanceId);
                    table.ForeignKey(
                        name: "FK_PayrollAllowances_Allowances_AllowanceId",
                        column: x => x.AllowanceId,
                        principalTable: "Allowances",
                        principalColumn: "AllowanceId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PayrollAllowances_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PayrollDeductions",
                columns: table => new
                {
                    PayrollDeductionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PayrollId = table.Column<int>(type: "int", nullable: false),
                    DeductionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollDeductions", x => x.PayrollDeductionId);
                    table.ForeignKey(
                        name: "FK_PayrollDeductions_Deductions_DeductionId",
                        column: x => x.DeductionId,
                        principalTable: "Deductions",
                        principalColumn: "DeductionId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PayrollDeductions_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PayGradeLevelId",
                table: "Employees",
                column: "PayGradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Allowances_PayGradeLevelId",
                table: "Allowances",
                column: "PayGradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_PayGradeLevelId",
                table: "Deductions",
                column: "PayGradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollAllowances_AllowanceId",
                table: "PayrollAllowances",
                column: "AllowanceId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollAllowances_PayrollId",
                table: "PayrollAllowances",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeductions_DeductionId",
                table: "PayrollDeductions",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeductions_PayrollId",
                table: "PayrollDeductions",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_EmployeeId",
                table: "Payrolls",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_OrganizationId",
                table: "Payrolls",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseManager_Employees_EmployeeId",
                table: "CaseManager",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseManager_Organizations_OrganizationId",
                table: "CaseManager",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_EmergencyClockOuts_CaseManager_CaseManagerId",
                table: "EmergencyClockOuts",
                column: "CaseManagerId",
                principalTable: "CaseManager",
                principalColumn: "CaseManagerId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_PayGradeLevels_PayGradeLevelId",
                table: "Employees",
                column: "PayGradeLevelId",
                principalTable: "PayGradeLevels",
                principalColumn: "PayGradeLevelId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_FireDrills_CaseManager_CaseManagerId",
                table: "FireDrills",
                column: "CaseManagerId",
                principalTable: "CaseManager",
                principalColumn: "CaseManagerId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollHistories_Payrolls_PayrollId",
                table: "PayrollHistories",
                column: "PayrollId",
                principalTable: "Payrolls",
                principalColumn: "PayrollId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseManager_Employees_EmployeeId",
                table: "CaseManager");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseManager_Organizations_OrganizationId",
                table: "CaseManager");

            migrationBuilder.DropForeignKey(
                name: "FK_EmergencyClockOuts_CaseManager_CaseManagerId",
                table: "EmergencyClockOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_PayGradeLevels_PayGradeLevelId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_FireDrills_CaseManager_CaseManagerId",
                table: "FireDrills");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollHistories_Payrolls_PayrollId",
                table: "PayrollHistories");

            migrationBuilder.DropTable(
                name: "PayrollAllowances");

            migrationBuilder.DropTable(
                name: "PayrollDeductions");

            migrationBuilder.DropTable(
                name: "Allowances");

            migrationBuilder.DropTable(
                name: "Deductions");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PayGradeLevelId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseManager",
                table: "CaseManager");

            migrationBuilder.DropColumn(
                name: "PayGradeLevelId",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "CaseManager",
                newName: "CaseManagers");

            migrationBuilder.RenameColumn(
                name: "PayrollId",
                table: "PayrollHistories",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollHistories_PayrollId",
                table: "PayrollHistories",
                newName: "IX_PayrollHistories_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseManager_OrganizationId",
                table: "CaseManagers",
                newName: "IX_CaseManagers_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseManager_EmployeeId",
                table: "CaseManagers",
                newName: "IX_CaseManagers_EmployeeId");

            migrationBuilder.AlterColumn<string>(
                name: "TransitCode",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankCode",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GrossPay",
                table: "PayrollHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NetPay",
                table: "PayrollHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OvertimeHoursWorked",
                table: "PayrollHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PayPeriodEndDate",
                table: "PayrollHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PayPeriodStartDate",
                table: "PayrollHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "PayrollHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RegularHoursWorked",
                table: "PayrollHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalDeduction",
                table: "PayrollHistories",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "HourlyRate",
                table: "PayGradeLevels",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseManagers",
                table: "CaseManagers",
                column: "CaseManagerId");

            migrationBuilder.CreateTable(
                name: "PayGradeLevelDeductions",
                columns: table => new
                {
                    PayGradeLevelDeductionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayGradeLevelId = table.Column<int>(type: "int", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HealthInsurance = table.Column<double>(type: "float", nullable: false),
                    RetirementContribution = table.Column<double>(type: "float", nullable: false),
                    TaxRate = table.Column<double>(type: "float", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayGradeLevelDeductions", x => x.PayGradeLevelDeductionId);
                    table.ForeignKey(
                        name: "FK_PayGradeLevelDeductions_PayGradeLevels_PayGradeLevelId",
                        column: x => x.PayGradeLevelId,
                        principalTable: "PayGradeLevels",
                        principalColumn: "PayGradeLevelId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayGradeLevelDeductions_PayGradeLevelId",
                table: "PayGradeLevelDeductions",
                column: "PayGradeLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseManagers_Employees_EmployeeId",
                table: "CaseManagers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseManagers_Organizations_OrganizationId",
                table: "CaseManagers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_EmergencyClockOuts_CaseManagers_CaseManagerId",
                table: "EmergencyClockOuts",
                column: "CaseManagerId",
                principalTable: "CaseManagers",
                principalColumn: "CaseManagerId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_FireDrills_CaseManagers_CaseManagerId",
                table: "FireDrills",
                column: "CaseManagerId",
                principalTable: "CaseManagers",
                principalColumn: "CaseManagerId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollHistories_Employees_EmployeeId",
                table: "PayrollHistories",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}

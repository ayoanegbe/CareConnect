using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.CommonLogic.Migrations
{
    public partial class Update10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Max",
                table: "LeaveSettings",
                newName: "PayGradeId");

            migrationBuilder.AddColumn<int>(
                name: "MaxCarryForward",
                table: "LeaveSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveSettings_PayGradeId",
                table: "LeaveSettings",
                column: "PayGradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveSettings_PayGrades_PayGradeId",
                table: "LeaveSettings",
                column: "PayGradeId",
                principalTable: "PayGrades",
                principalColumn: "PayGradeId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveSettings_PayGrades_PayGradeId",
                table: "LeaveSettings");

            migrationBuilder.DropIndex(
                name: "IX_LeaveSettings_PayGradeId",
                table: "LeaveSettings");

            migrationBuilder.DropColumn(
                name: "MaxCarryForward",
                table: "LeaveSettings");

            migrationBuilder.RenameColumn(
                name: "PayGradeId",
                table: "LeaveSettings",
                newName: "Max");
        }
    }
}

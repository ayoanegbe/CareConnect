using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.CommonLogic.Migrations
{
    public partial class Update9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveSettings_PayGradeLevels_PayGradeLevelId",
                table: "LeaveSettings");

            migrationBuilder.DropIndex(
                name: "IX_LeaveSettings_PayGradeLevelId",
                table: "LeaveSettings");

            migrationBuilder.DropColumn(
                name: "PayGradeLevelId",
                table: "LeaveSettings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PayGradeLevelId",
                table: "LeaveSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveSettings_PayGradeLevelId",
                table: "LeaveSettings",
                column: "PayGradeLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveSettings_PayGradeLevels_PayGradeLevelId",
                table: "LeaveSettings",
                column: "PayGradeLevelId",
                principalTable: "PayGradeLevels",
                principalColumn: "PayGradeLevelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

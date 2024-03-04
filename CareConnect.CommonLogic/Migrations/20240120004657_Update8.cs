using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.CommonLogic.Migrations
{
    public partial class Update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaidLeave",
                table: "LeaveSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Max",
                table: "LeaveSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaidLeave",
                table: "LeaveSettings");

            migrationBuilder.DropColumn(
                name: "Max",
                table: "LeaveSettings");
        }
    }
}

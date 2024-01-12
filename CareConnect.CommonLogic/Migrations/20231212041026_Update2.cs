using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.CommonLogic.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumbersRequired",
                table: "ShiftRun");

            migrationBuilder.AddColumn<int>(
                name: "NumbersRequired",
                table: "Shifts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumbersRequired",
                table: "Shifts");

            migrationBuilder.AddColumn<int>(
                name: "NumbersRequired",
                table: "ShiftRun",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

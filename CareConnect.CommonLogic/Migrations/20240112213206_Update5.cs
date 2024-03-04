using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.CommonLogic.Migrations
{
    public partial class Update5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Relationship",
                table: "Clients",
                newName: "EmergencyContactRelationship");

            migrationBuilder.AddColumn<int>(
                name: "ContactPersonRelationship",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPersonRelationship",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "EmergencyContactRelationship",
                table: "Clients",
                newName: "Relationship");
        }
    }
}

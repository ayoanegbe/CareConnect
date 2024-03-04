using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.CommonLogic.Migrations
{
    public partial class Update11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Leaves",
                newName: "LeaveSettingId");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Leaves",
                newName: "RejectReason");

            migrationBuilder.AddColumn<string>(
                name: "LeaveReason",
                table: "Leaves",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveSettingId",
                table: "Leaves",
                column: "LeaveSettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_LeaveSettings_LeaveSettingId",
                table: "Leaves",
                column: "LeaveSettingId",
                principalTable: "LeaveSettings",
                principalColumn: "LeaveSettingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_LeaveSettings_LeaveSettingId",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_LeaveSettingId",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "LeaveReason",
                table: "Leaves");

            migrationBuilder.RenameColumn(
                name: "RejectReason",
                table: "Leaves",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "LeaveSettingId",
                table: "Leaves",
                newName: "Type");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.CommonLogic.Migrations
{
    public partial class Update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssigments_ShiftRun_ShiftRunId",
                table: "ShiftAssigments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftRun_Shifts_ShiftId",
                table: "ShiftRun");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftRun",
                table: "ShiftRun");

            migrationBuilder.RenameTable(
                name: "ShiftRun",
                newName: "ShiftRuns");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftRun_ShiftId",
                table: "ShiftRuns",
                newName: "IX_ShiftRuns_ShiftId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftRuns",
                table: "ShiftRuns",
                column: "ShiftRunId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssigments_ShiftRuns_ShiftRunId",
                table: "ShiftAssigments",
                column: "ShiftRunId",
                principalTable: "ShiftRuns",
                principalColumn: "ShiftRunId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftRuns_Shifts_ShiftId",
                table: "ShiftRuns",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssigments_ShiftRuns_ShiftRunId",
                table: "ShiftAssigments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftRuns_Shifts_ShiftId",
                table: "ShiftRuns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftRuns",
                table: "ShiftRuns");

            migrationBuilder.RenameTable(
                name: "ShiftRuns",
                newName: "ShiftRun");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftRuns_ShiftId",
                table: "ShiftRun",
                newName: "IX_ShiftRun_ShiftId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftRun",
                table: "ShiftRun",
                column: "ShiftRunId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssigments_ShiftRun_ShiftRunId",
                table: "ShiftAssigments",
                column: "ShiftRunId",
                principalTable: "ShiftRun",
                principalColumn: "ShiftRunId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftRun_Shifts_ShiftId",
                table: "ShiftRun",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}

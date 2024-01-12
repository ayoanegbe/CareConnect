using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareConnect.CommonLogic.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssigments_Shifts_ShiftId",
                table: "ShiftAssigments");

            migrationBuilder.DropColumn(
                name: "NumbersAssigned",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "NumbersRequired",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ShiftAssigments");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ShiftAssigments");

            migrationBuilder.RenameColumn(
                name: "IsAssigned",
                table: "Shifts",
                newName: "Wednesday");

            migrationBuilder.RenameColumn(
                name: "ShiftId",
                table: "ShiftAssigments",
                newName: "ShiftRunId");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftAssigments_ShiftId",
                table: "ShiftAssigments",
                newName: "IX_ShiftAssigments_ShiftRunId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Shifts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Friday",
                table: "Shifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Monday",
                table: "Shifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Saturday",
                table: "Shifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Sunday",
                table: "Shifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Thursday",
                table: "Shifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Tuesday",
                table: "Shifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "ShiftPatterns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "ShiftPatterns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ShiftRun",
                columns: table => new
                {
                    ShiftRunId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    ShiftDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAssigned = table.Column<bool>(type: "bit", nullable: false),
                    NumbersRequired = table.Column<int>(type: "int", nullable: false),
                    NumbersAssigned = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftRun", x => x.ShiftRunId);
                    table.ForeignKey(
                        name: "FK_ShiftRun_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftRun_ShiftId",
                table: "ShiftRun",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssigments_ShiftRun_ShiftRunId",
                table: "ShiftAssigments",
                column: "ShiftRunId",
                principalTable: "ShiftRun",
                principalColumn: "ShiftRunId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssigments_ShiftRun_ShiftRunId",
                table: "ShiftAssigments");

            migrationBuilder.DropTable(
                name: "ShiftRun");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Friday",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Monday",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Saturday",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Sunday",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Thursday",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Tuesday",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "Wednesday",
                table: "Shifts",
                newName: "IsAssigned");

            migrationBuilder.RenameColumn(
                name: "ShiftRunId",
                table: "ShiftAssigments",
                newName: "ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftAssigments_ShiftRunId",
                table: "ShiftAssigments",
                newName: "IX_ShiftAssigments_ShiftId");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "ShiftPatterns",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "ShiftPatterns",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ShiftAssigments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ShiftAssigments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssigments_Shifts_ShiftId",
                table: "ShiftAssigments",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}

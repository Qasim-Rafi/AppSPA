using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Leaves_LeaveApprovalTypeId",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "Discription",
                table: "LeaveApprovalType");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Leaves",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Leaves",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LeaveApprovalType",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveApprovalTypeId",
                table: "Leaves",
                column: "LeaveApprovalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves",
                column: "LeaveTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Leaves_LeaveApprovalTypeId",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LeaveApprovalType");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Leaves",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discription",
                table: "LeaveApprovalType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveApprovalTypeId",
                table: "Leaves",
                column: "LeaveApprovalTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves",
                column: "LeaveTypeId",
                unique: true);
        }
    }
}

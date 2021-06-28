using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateAttendance_NoticeBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApproveById",
                table: "NoticeBoards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproveComment",
                table: "NoticeBoards",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDateTime",
                table: "NoticeBoards",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "NoticeBoards",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Leave",
                table: "Attendances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LeaveDescription",
                table: "Attendances",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveById",
                table: "NoticeBoards");

            migrationBuilder.DropColumn(
                name: "ApproveComment",
                table: "NoticeBoards");

            migrationBuilder.DropColumn(
                name: "ApproveDateTime",
                table: "NoticeBoards");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "NoticeBoards");

            migrationBuilder.DropColumn(
                name: "Leave",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "LeaveDescription",
                table: "Attendances");
        }
    }
}

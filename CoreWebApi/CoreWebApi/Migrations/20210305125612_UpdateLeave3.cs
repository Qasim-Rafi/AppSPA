using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateLeave3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApproveById",
                table: "Leaves",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproveComment",
                table: "Leaves",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDateTime",
                table: "Leaves",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveById",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "ApproveComment",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "ApproveDateTime",
                table: "Leaves");
        }
    }
}

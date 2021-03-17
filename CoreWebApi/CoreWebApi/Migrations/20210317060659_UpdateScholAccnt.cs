using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateScholAccnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SchoolCashAccount");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SchoolCashAccount",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "SchoolCashAccount");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "SchoolCashAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

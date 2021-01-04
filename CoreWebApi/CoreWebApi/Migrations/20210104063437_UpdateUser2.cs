using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentContactNumber",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentEmail",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentContactNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ParentEmail",
                table: "Users");
        }
    }
}

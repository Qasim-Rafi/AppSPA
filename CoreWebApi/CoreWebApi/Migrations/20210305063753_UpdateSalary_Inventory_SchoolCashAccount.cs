using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSalary_Inventory_SchoolCashAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "StaffInventory");

            migrationBuilder.AddColumn<bool>(
                name: "Posted",
                table: "EmployeeSalaries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Posted",
                table: "EmployeeSalaries");

            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "StaffInventory",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

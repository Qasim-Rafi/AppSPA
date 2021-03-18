using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSemesterFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallmentAmount",
                table: "SemesterFeeMappings");

            migrationBuilder.AddColumn<int>(
                name: "Installments",
                table: "SemesterFeeMappings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Installments",
                table: "SemesterFeeMappings");

            migrationBuilder.AddColumn<double>(
                name: "InstallmentAmount",
                table: "SemesterFeeMappings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

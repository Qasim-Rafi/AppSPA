using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Subjects");

            migrationBuilder.AlterColumn<int>(
                name: "CreditHours",
                table: "Subjects",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Subjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpertRank",
                table: "Subjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ExpertRank",
                table: "Subjects");

            migrationBuilder.AlterColumn<int>(
                name: "CreditHours",
                table: "Subjects",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

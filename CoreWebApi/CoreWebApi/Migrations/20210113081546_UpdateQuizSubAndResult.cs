using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateQuizSubAndResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultMarks",
                table: "Results");

            migrationBuilder.AlterColumn<double>(
                name: "TotalMarks",
                table: "Results",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<double>(
                name: "ObtainedMarks",
                table: "Results",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ResultMarks",
                table: "QuizSubmissions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObtainedMarks",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "ResultMarks",
                table: "QuizSubmissions");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalMarks",
                table: "Results",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<decimal>(
                name: "ResultMarks",
                table: "Results",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

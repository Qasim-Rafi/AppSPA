using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateResult_TutorStdMaping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "TutorStudentMappings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ReferenceId",
                table: "Results",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClassSectionId",
                table: "Results",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TutorExamName",
                table: "Results",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "TutorStudentMappings");

            migrationBuilder.DropColumn(
                name: "TutorExamName",
                table: "Results");

            migrationBuilder.AlterColumn<int>(
                name: "ReferenceId",
                table: "Results",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClassSectionId",
                table: "Results",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

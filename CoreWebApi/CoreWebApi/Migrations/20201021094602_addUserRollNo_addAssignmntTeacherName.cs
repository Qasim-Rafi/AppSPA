using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addUserRollNo_addAssignmntTeacherName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RollNumber",
                table: "Users",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionId",
                table: "Quizzes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TeacherName",
                table: "Assignments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ClassSectionId",
                table: "Quizzes",
                column: "ClassSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_ClassSections_ClassSectionId",
                table: "Quizzes",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_ClassSections_ClassSectionId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_ClassSectionId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "RollNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TeacherName",
                table: "Assignments");
        }
    }
}

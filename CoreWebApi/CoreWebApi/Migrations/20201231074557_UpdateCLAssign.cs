using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateCLAssign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId",
                table: "ClassLectureAssignment");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "ClassLectureAssignment",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_LectureId",
                table: "ClassLectureAssignment",
                column: "LectureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_LectureId",
                table: "ClassLectureAssignment");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "ClassLectureAssignment",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId",
                table: "ClassLectureAssignment",
                columns: new[] { "LectureId", "TeacherId" },
                unique: true);
        }
    }
}

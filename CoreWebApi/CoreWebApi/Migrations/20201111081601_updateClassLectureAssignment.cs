using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class updateClassLectureAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassLectureAssignment",
                table: "ClassLectureAssignment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassLectureAssignment",
                table: "ClassLectureAssignment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId",
                table: "ClassLectureAssignment",
                columns: new[] { "LectureId", "TeacherId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassLectureAssignment",
                table: "ClassLectureAssignment");

            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId",
                table: "ClassLectureAssignment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassLectureAssignment",
                table: "ClassLectureAssignment",
                columns: new[] { "LectureId", "TeacherId" });
        }
    }
}

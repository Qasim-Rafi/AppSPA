using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateClassLecAssignCompPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_LectureId",
                table: "ClassLectureAssignment");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId",
                table: "ClassLectureAssignment",
                columns: new[] { "LectureId", "TeacherId" },
                unique: true,
                filter: "[TeacherId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId",
                table: "ClassLectureAssignment");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_LectureId",
                table: "ClassLectureAssignment",
                column: "LectureId");
        }
    }
}

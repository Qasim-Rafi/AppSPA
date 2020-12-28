using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateLectureAssign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassLectureAssignment_ClassSections_ClassSectionId",
                table: "ClassLectureAssignment");

            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_ClassSectionId",
                table: "ClassLectureAssignment");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "ClassLectureAssignment");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "ClassLectureAssignment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_ClassId",
                table: "ClassLectureAssignment",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassLectureAssignment_Class_ClassId",
                table: "ClassLectureAssignment",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassLectureAssignment_Class_ClassId",
                table: "ClassLectureAssignment");

            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_ClassId",
                table: "ClassLectureAssignment");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "ClassLectureAssignment");

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionId",
                table: "ClassLectureAssignment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_ClassSectionId",
                table: "ClassLectureAssignment",
                column: "ClassSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassLectureAssignment_ClassSections_ClassSectionId",
                table: "ClassLectureAssignment",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

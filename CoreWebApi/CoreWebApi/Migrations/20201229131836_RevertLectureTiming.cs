using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class RevertLectureTiming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_SectionId",
                table: "ClassSections",
                column: "SectionId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_Class_ClassId",
                table: "ClassSections",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_Sections_SectionId",
                table: "ClassSections",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassLectureAssignment_ClassSections_ClassSectionId",
                table: "ClassLectureAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_Class_ClassId",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_Sections_SectionId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_SectionId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_ClassSectionId",
                table: "ClassLectureAssignment");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "ClassLectureAssignment");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "ClassLectureAssignment",
                type: "int",
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
    }
}

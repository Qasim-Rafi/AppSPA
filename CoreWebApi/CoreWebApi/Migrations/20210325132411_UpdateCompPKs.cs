using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateCompPKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_ClassId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_ClassId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId",
                table: "ClassLectureAssignment");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolBranchId",
                table: "SubjectAssignments",
                columns: new[] { "ClassId", "SubjectId", "SchoolBranchId" },
                unique: true,
                filter: "[ClassId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_SemesterId_SubjectId_SchoolBranchId",
                table: "SubjectAssignments",
                columns: new[] { "SemesterId", "SubjectId", "SchoolBranchId" },
                unique: true,
                filter: "[SemesterId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_ClassId_SectionId_SchoolBranchId",
                table: "ClassSections",
                columns: new[] { "ClassId", "SectionId", "SchoolBranchId" },
                unique: true,
                filter: "[ClassId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_SemesterId_SectionId_SchoolBranchId",
                table: "ClassSections",
                columns: new[] { "SemesterId", "SectionId", "SchoolBranchId" },
                unique: true,
                filter: "[SemesterId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId_ClassSectionId",
                table: "ClassLectureAssignment",
                columns: new[] { "LectureId", "TeacherId", "ClassSectionId" },
                unique: true,
                filter: "[TeacherId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_Semesters_SemesterId",
                table: "ClassSections",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAssignments_Semesters_SemesterId",
                table: "SubjectAssignments",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_Semesters_SemesterId",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAssignments_Semesters_SemesterId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolBranchId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_SemesterId_SubjectId_SchoolBranchId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_ClassId_SectionId_SchoolBranchId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_SemesterId_SectionId_SchoolBranchId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId_ClassSectionId",
                table: "ClassLectureAssignment");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_ClassId",
                table: "SubjectAssignments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_ClassId",
                table: "ClassSections",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_LectureId_TeacherId",
                table: "ClassLectureAssignment",
                columns: new[] { "LectureId", "TeacherId" },
                unique: true,
                filter: "[TeacherId] IS NOT NULL");
        }
    }
}

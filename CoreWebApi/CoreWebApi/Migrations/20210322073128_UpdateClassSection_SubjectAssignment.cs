using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateClassSection_SubjectAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolBranchId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_ClassId_SectionId_SchoolBranchId",
                table: "ClassSections");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "SubjectAssignments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "SubjectAssignments",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "ClassSections",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "ClassSections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_ClassId",
                table: "SubjectAssignments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_ClassId",
                table: "ClassSections",
                column: "ClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_ClassId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_ClassId",
                table: "ClassSections");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "SubjectAssignments");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "ClassSections");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "SubjectAssignments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "ClassSections",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolBranchId",
                table: "SubjectAssignments",
                columns: new[] { "ClassId", "SubjectId", "SchoolBranchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_ClassId_SectionId_SchoolBranchId",
                table: "ClassSections",
                columns: new[] { "ClassId", "SectionId", "SchoolBranchId" },
                unique: true);
        }
    }
}

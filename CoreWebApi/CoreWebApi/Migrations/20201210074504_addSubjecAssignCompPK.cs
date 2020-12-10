using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addSubjecAssignCompPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_ClassId",
                table: "SubjectAssignments");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolId",
                table: "SubjectAssignments",
                columns: new[] { "ClassId", "SubjectId", "SchoolId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolId",
                table: "SubjectAssignments");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_ClassId",
                table: "SubjectAssignments",
                column: "ClassId");
        }
    }
}

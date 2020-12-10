using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addCSCompPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_ClassId_SectionId_SchoolBranchId",
                table: "ClassSections",
                columns: new[] { "ClassId", "SectionId", "SchoolBranchId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassSections_ClassId_SectionId_SchoolBranchId",
                table: "ClassSections");
        }
    }
}

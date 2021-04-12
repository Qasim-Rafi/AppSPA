using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSubjectCompPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_Name_SchoolBranchId",
                table: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Name_SchoolBranchId_CreatedById",
                table: "Subjects",
                columns: new[] { "Name", "SchoolBranchId", "CreatedById" },
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_Name_SchoolBranchId_CreatedById",
                table: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Name_SchoolBranchId",
                table: "Subjects",
                columns: new[] { "Name", "SchoolBranchId" },
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}

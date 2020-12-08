using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class updateCSUserCompositePK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassSectionUsers_SchoolBranchId",
                table: "ClassSectionUsers");

            migrationBuilder.DropIndex(
                name: "IX_ClassSectionUsers_ClassSectionId_UserId",
                table: "ClassSectionUsers");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionUsers_ClassSectionId",
                table: "ClassSectionUsers",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionUsers_SchoolBranchId_UserId",
                table: "ClassSectionUsers",
                columns: new[] { "SchoolBranchId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassSectionUsers_ClassSectionId",
                table: "ClassSectionUsers");

            migrationBuilder.DropIndex(
                name: "IX_ClassSectionUsers_SchoolBranchId_UserId",
                table: "ClassSectionUsers");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionUsers_SchoolBranchId",
                table: "ClassSectionUsers",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionUsers_ClassSectionId_UserId",
                table: "ClassSectionUsers",
                columns: new[] { "ClassSectionId", "UserId" },
                unique: true);
        }
    }
}

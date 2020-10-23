using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class updateCSUserRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionUsers_ClassSections_ClassSectionId",
                table: "ClassSectionUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionUsers_ClassSections_ClassSectionId",
                table: "ClassSectionUsers",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionUsers_ClassSections_ClassSectionId",
                table: "ClassSectionUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionUsers_ClassSections_ClassSectionId",
                table: "ClassSectionUsers",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addGroupCSID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassSectionId",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ClassSectionId",
                table: "Groups",
                column: "ClassSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_ClassSections_ClassSectionId",
                table: "Groups",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_ClassSections_ClassSectionId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ClassSectionId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "Groups");
        }
    }
}

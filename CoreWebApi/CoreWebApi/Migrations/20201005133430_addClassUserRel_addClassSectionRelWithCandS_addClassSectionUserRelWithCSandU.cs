using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addClassUserRel_addClassSectionRelWithCandS_addClassSectionUserRelWithCSandU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Class_ClassSections_ClassSectionId",
                table: "Class");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_ClassSectionUsers_ClassSectionUserId",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_ClassSections_ClassSectionId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ClassSectionUsers_ClassSectionUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ClassSectionUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Sections_ClassSectionId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_ClassSectionUserId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_Class_ClassSectionId",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "ClassSectionUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "ClassSectionUserId",
                table: "ClassSections");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "Class");

            migrationBuilder.CreateIndex(
                name: "IX_Class_CreatedById",
                table: "Class",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Users_CreatedById",
                table: "Class",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Class_Users_CreatedById",
                table: "Class");

            migrationBuilder.DropIndex(
                name: "IX_Class_CreatedById",
                table: "Class");

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionUserId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionId",
                table: "Sections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionUserId",
                table: "ClassSections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionId",
                table: "Class",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClassSectionUserId",
                table: "Users",
                column: "ClassSectionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_ClassSectionId",
                table: "Sections",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_ClassSectionUserId",
                table: "ClassSections",
                column: "ClassSectionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Class_ClassSectionId",
                table: "Class",
                column: "ClassSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Class_ClassSections_ClassSectionId",
                table: "Class",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_ClassSectionUsers_ClassSectionUserId",
                table: "ClassSections",
                column: "ClassSectionUserId",
                principalTable: "ClassSectionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_ClassSections_ClassSectionId",
                table: "Sections",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ClassSectionUsers_ClassSectionUserId",
                table: "Users",
                column: "ClassSectionUserId",
                principalTable: "ClassSectionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

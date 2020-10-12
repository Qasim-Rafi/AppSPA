using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addCSectionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchoolAcademyId",
                table: "ClassSections",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionUsers_ClassSectionId",
                table: "ClassSectionUsers",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionUsers_UserId",
                table: "ClassSectionUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_ClassId",
                table: "ClassSections",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_SchoolAcademyId",
                table: "ClassSections",
                column: "SchoolAcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_SectionId",
                table: "ClassSections",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_Class_ClassId",
                table: "ClassSections",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_SchoolAcademy_SchoolAcademyId",
                table: "ClassSections",
                column: "SchoolAcademyId",
                principalTable: "SchoolAcademy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_Sections_SectionId",
                table: "ClassSections",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionUsers_ClassSections_ClassSectionId",
                table: "ClassSectionUsers",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionUsers_Users_UserId",
                table: "ClassSectionUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_Class_ClassId",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_SchoolAcademy_SchoolAcademyId",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_Sections_SectionId",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionUsers_ClassSections_ClassSectionId",
                table: "ClassSectionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionUsers_Users_UserId",
                table: "ClassSectionUsers");

            migrationBuilder.DropIndex(
                name: "IX_ClassSectionUsers_ClassSectionId",
                table: "ClassSectionUsers");

            migrationBuilder.DropIndex(
                name: "IX_ClassSectionUsers_UserId",
                table: "ClassSectionUsers");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_ClassId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_SchoolAcademyId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_SectionId",
                table: "ClassSections");

            migrationBuilder.DropColumn(
                name: "SchoolAcademyId",
                table: "ClassSections");
        }
    }
}

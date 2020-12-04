using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class updateCSCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchoolAcademyId",
                table: "ClassSections");

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "ClassSections",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_SchoolBranchId",
                table: "ClassSections",
                column: "SchoolBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_SchoolBranch_SchoolBranchId",
                table: "ClassSections",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_SchoolBranch_SchoolBranchId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_SchoolBranchId",
                table: "ClassSections");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "ClassSections");

            migrationBuilder.AddColumn<int>(
                name: "SchoolAcademyId",
                table: "ClassSections",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateResource3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "UsefulResources",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsefulResources_SchoolBranchId",
                table: "UsefulResources",
                column: "SchoolBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsefulResources_SchoolBranch_SchoolBranchId",
                table: "UsefulResources",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsefulResources_SchoolBranch_SchoolBranchId",
                table: "UsefulResources");

            migrationBuilder.DropIndex(
                name: "IX_UsefulResources_SchoolBranchId",
                table: "UsefulResources");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "UsefulResources");
        }
    }
}

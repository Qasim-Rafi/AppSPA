using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "StudentFees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentFees_SchoolBranchId",
                table: "StudentFees",
                column: "SchoolBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentFees_SchoolBranch_SchoolBranchId",
                table: "StudentFees",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentFees_SchoolBranch_SchoolBranchId",
                table: "StudentFees");

            migrationBuilder.DropIndex(
                name: "IX_StudentFees_SchoolBranchId",
                table: "StudentFees");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "StudentFees");
        }
    }
}

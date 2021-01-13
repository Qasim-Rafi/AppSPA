using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSubjectAssign2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAssignments_SchoolBranch_SchoolId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_SchoolId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolId",
                table: "SubjectAssignments");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "SubjectAssignments");

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "SubjectAssignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_SchoolBranchId",
                table: "SubjectAssignments",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolBranchId",
                table: "SubjectAssignments",
                columns: new[] { "ClassId", "SubjectId", "SchoolBranchId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAssignments_SchoolBranch_SchoolBranchId",
                table: "SubjectAssignments",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAssignments_SchoolBranch_SchoolBranchId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_SchoolBranchId",
                table: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolBranchId",
                table: "SubjectAssignments");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "SubjectAssignments");

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "SubjectAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_SchoolId",
                table: "SubjectAssignments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_ClassId_SubjectId_SchoolId",
                table: "SubjectAssignments",
                columns: new[] { "ClassId", "SubjectId", "SchoolId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAssignments_SchoolBranch_SchoolId",
                table: "SubjectAssignments",
                column: "SchoolId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

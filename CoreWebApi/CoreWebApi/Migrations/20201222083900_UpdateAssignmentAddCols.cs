using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateAssignmentAddCols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropColumn(
                name: "CreatedByDatetime",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "EndDatetime",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "ReferenceMaterial",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "StartDatetime",
                table: "ClassSectionAssignment");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentName",
                table: "ClassSectionAssignment",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ClassSectionAssignment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "ClassSectionAssignment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDateTime",
                table: "ClassSectionAssignment",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "ClassSectionAssignment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceUrl",
                table: "ClassSectionAssignment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedMaterial",
                table: "ClassSectionAssignment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "ClassSectionAssignment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionAssignment_CreatedById",
                table: "ClassSectionAssignment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionAssignment_SchoolBranchId",
                table: "ClassSectionAssignment",
                column: "SchoolBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionAssignment_Users_CreatedById",
                table: "ClassSectionAssignment",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionAssignment_SchoolBranch_SchoolBranchId",
                table: "ClassSectionAssignment",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionAssignment_Users_CreatedById",
                table: "ClassSectionAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionAssignment_SchoolBranch_SchoolBranchId",
                table: "ClassSectionAssignment");

            migrationBuilder.DropIndex(
                name: "IX_ClassSectionAssignment_CreatedById",
                table: "ClassSectionAssignment");

            migrationBuilder.DropIndex(
                name: "IX_ClassSectionAssignment_SchoolBranchId",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "AssignmentName",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "DueDateTime",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "ReferenceUrl",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "RelatedMaterial",
                table: "ClassSectionAssignment");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "ClassSectionAssignment");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedByDatetime",
                table: "ClassSectionAssignment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ClassSectionAssignment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDatetime",
                table: "ClassSectionAssignment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ClassSectionAssignment",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceMaterial",
                table: "ClassSectionAssignment",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDatetime",
                table: "ClassSectionAssignment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ClassSectionId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolBranchId = table.Column<int>(type: "int", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_ClassSections_ClassSectionId",
                        column: x => x.ClassSectionId,
                        principalTable: "ClassSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ClassSectionId",
                table: "Assignments",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CreatedById",
                table: "Assignments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SchoolBranchId",
                table: "Assignments",
                column: "SchoolBranchId");
        }
    }
}

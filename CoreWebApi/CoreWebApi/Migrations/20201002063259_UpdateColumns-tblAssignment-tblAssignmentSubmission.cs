using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateColumnstblAssignmenttblAssignmentSubmission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Class_ClassId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionAssigmentSubmissions_ClassSectionAssignment_classSectionAssignmentId",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ClassSectionAssigmentSubmissions_classSectionAssignmentId",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_ClassId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ClassAssignmentSectionId",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropColumn(
                name: "classSectionAssignmentId",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropColumn(
                name: "dateTime",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropColumn(
                name: "documentPath",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Creationdatetime",
                table: "Assignments");

            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "ClassSectionAssigmentSubmissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ClassSectionAssigmentSubmissions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SubmittedMaterial",
                table: "ClassSectionAssigmentSubmissions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Assignments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionAssigmentSubmissions_AssignmentId",
                table: "ClassSectionAssigmentSubmissions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ClassSectionId",
                table: "Assignments",
                column: "ClassSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_ClassSections_ClassSectionId",
                table: "Assignments",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionAssigmentSubmissions_Assignments_AssignmentId",
                table: "ClassSectionAssigmentSubmissions",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_ClassSections_ClassSectionId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionAssigmentSubmissions_Assignments_AssignmentId",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ClassSectionAssigmentSubmissions_AssignmentId",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_ClassSectionId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropColumn(
                name: "SubmittedMaterial",
                table: "ClassSectionAssigmentSubmissions");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Assignments");

            migrationBuilder.AddColumn<int>(
                name: "ClassAssignmentSectionId",
                table: "ClassSectionAssigmentSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "classSectionAssignmentId",
                table: "ClassSectionAssigmentSubmissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateTime",
                table: "ClassSectionAssigmentSubmissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "documentPath",
                table: "ClassSectionAssigmentSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Assignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creationdatetime",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionAssigmentSubmissions_classSectionAssignmentId",
                table: "ClassSectionAssigmentSubmissions",
                column: "classSectionAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ClassId",
                table: "Assignments",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Class_ClassId",
                table: "Assignments",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionAssigmentSubmissions_ClassSectionAssignment_classSectionAssignmentId",
                table: "ClassSectionAssigmentSubmissions",
                column: "classSectionAssignmentId",
                principalTable: "ClassSectionAssignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

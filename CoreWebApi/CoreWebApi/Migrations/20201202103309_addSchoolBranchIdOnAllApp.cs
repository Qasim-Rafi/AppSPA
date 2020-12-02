using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addSchoolBranchIdOnAllApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Class_Subjects_SubjectId",
                table: "Class");

            migrationBuilder.DropIndex(
                name: "IX_Class_SubjectId",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Class");

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "UploadedLectures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Subjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Subjects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "Subjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "Sessions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "Quizzes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "Leaves",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "Attendances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UploadedLectures_SchoolBranchId",
                table: "UploadedLectures",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SchoolBranchId",
                table: "Subjects",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SchoolBranchId",
                table: "Sessions",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_SchoolBranchId",
                table: "Quizzes",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_SchoolBranchId",
                table: "Leaves",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Class_SchoolBranchId",
                table: "Class",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SchoolBranchId",
                table: "Attendances",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SchoolBranchId",
                table: "Assignments",
                column: "SchoolBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_SchoolBranch_SchoolBranchId",
                table: "Assignments",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_SchoolBranch_SchoolBranchId",
                table: "Attendances",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Class_SchoolBranch_SchoolBranchId",
                table: "Class",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_SchoolBranch_SchoolBranchId",
                table: "Leaves",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_SchoolBranch_SchoolBranchId",
                table: "Quizzes",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_SchoolBranch_SchoolBranchId",
                table: "Sessions",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_SchoolBranch_SchoolBranchId",
                table: "Subjects",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UploadedLectures_SchoolBranch_SchoolBranchId",
                table: "UploadedLectures",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_SchoolBranch_SchoolBranchId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_SchoolBranch_SchoolBranchId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Class_SchoolBranch_SchoolBranchId",
                table: "Class");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_SchoolBranch_SchoolBranchId",
                table: "Leaves");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_SchoolBranch_SchoolBranchId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_SchoolBranch_SchoolBranchId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_SchoolBranch_SchoolBranchId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_UploadedLectures_SchoolBranch_SchoolBranchId",
                table: "UploadedLectures");

            migrationBuilder.DropIndex(
                name: "IX_UploadedLectures_SchoolBranchId",
                table: "UploadedLectures");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SchoolBranchId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_SchoolBranchId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_SchoolBranchId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_SchoolBranchId",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Class_SchoolBranchId",
                table: "Class");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_SchoolBranchId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_SchoolBranchId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "UploadedLectures");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "Assignments");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Class",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Class_SubjectId",
                table: "Class",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Subjects_SubjectId",
                table: "Class",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

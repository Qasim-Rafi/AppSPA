using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class updateSubject_addSubjectAssign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectContents_Subjects_SubjectId",
                table: "SubjectContents");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Class_ClassId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Users_CreatedById",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_SchoolBranch_SchoolId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_ClassId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_CreatedById",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SchoolId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_SubjectContents_SubjectId",
                table: "SubjectContents");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "SubjectContents");

            migrationBuilder.AddColumn<int>(
                name: "SubjectAssignmentId",
                table: "SubjectContents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Class",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(nullable: false),
                    ClassId = table.Column<int>(nullable: false),
                    SchoolId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectAssignments_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectAssignments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectAssignments_SchoolBranch_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectAssignments_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectContents_SubjectAssignmentId",
                table: "SubjectContents",
                column: "SubjectAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Class_SubjectId",
                table: "Class",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_ClassId",
                table: "SubjectAssignments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_CreatedById",
                table: "SubjectAssignments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_SchoolId",
                table: "SubjectAssignments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignments_SubjectId",
                table: "SubjectAssignments",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Subjects_SubjectId",
                table: "Class",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectContents_SubjectAssignments_SubjectAssignmentId",
                table: "SubjectContents",
                column: "SubjectAssignmentId",
                principalTable: "SubjectAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Class_Subjects_SubjectId",
                table: "Class");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectContents_SubjectAssignments_SubjectAssignmentId",
                table: "SubjectContents");

            migrationBuilder.DropTable(
                name: "SubjectAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SubjectContents_SubjectAssignmentId",
                table: "SubjectContents");

            migrationBuilder.DropIndex(
                name: "IX_Class_SubjectId",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "SubjectAssignmentId",
                table: "SubjectContents");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Class");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Subjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "SubjectContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ClassId",
                table: "Subjects",
                column: "ClassId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CreatedById",
                table: "Subjects",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SchoolId",
                table: "Subjects",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectContents_SubjectId",
                table: "SubjectContents",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectContents_Subjects_SubjectId",
                table: "SubjectContents",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Class_ClassId",
                table: "Subjects",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Users_CreatedById",
                table: "Subjects",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_SchoolBranch_SchoolId",
                table: "Subjects",
                column: "SchoolId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

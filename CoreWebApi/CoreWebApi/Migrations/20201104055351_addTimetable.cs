using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addTimetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LectureTiming",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolBranchId = table.Column<int>(nullable: false),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false),
                    IsBreak = table.Column<bool>(nullable: false),
                    Day = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureTiming", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureTiming_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassLectureAssignment",
                columns: table => new
                {
                    LectureId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSectionId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLectureAssignment", x => new { x.LectureId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_ClassLectureAssignment_ClassSections_ClassSectionId",
                        column: x => x.ClassSectionId,
                        principalTable: "ClassSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassLectureAssignment_LectureTiming_LectureId",
                        column: x => x.LectureId,
                        principalTable: "LectureTiming",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassLectureAssignment_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassLectureAssignment_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_ClassSectionId",
                table: "ClassLectureAssignment",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_SubjectId",
                table: "ClassLectureAssignment",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLectureAssignment_TeacherId",
                table: "ClassLectureAssignment",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureTiming_SchoolBranchId",
                table: "LectureTiming",
                column: "SchoolBranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassLectureAssignment");

            migrationBuilder.DropTable(
                name: "LectureTiming");
        }
    }
}

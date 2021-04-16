using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class SeperateTutorSubject_Attendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpertRate",
                table: "Subjects");

            migrationBuilder.AlterColumn<int>(
                name: "CreditHours",
                table: "Subjects",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TutorAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(nullable: true),
                    SubjectId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    Present = table.Column<bool>(nullable: false),
                    Absent = table.Column<bool>(nullable: false),
                    Late = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    CreatedDatetime = table.Column<DateTime>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorAttendances_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TutorSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    ExpertRate = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorSubjects_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TutorAttendances_SchoolBranchId",
                table: "TutorAttendances",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSubjects_SchoolBranchId",
                table: "TutorSubjects",
                column: "SchoolBranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TutorAttendances");

            migrationBuilder.DropTable(
                name: "TutorSubjects");

            migrationBuilder.AlterColumn<int>(
                name: "CreditHours",
                table: "Subjects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ExpertRate",
                table: "Subjects",
                type: "int",
                nullable: true);
        }
    }
}

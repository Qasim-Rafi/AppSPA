using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddTeacherExp_ExpTrans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeacherExperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<int>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false),
                    LevelFrom = table.Column<int>(nullable: false),
                    LevelTo = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherExperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherExperties_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherExperties_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherExperties_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherExperties_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherExpertiesTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherExpertiesId = table.Column<int>(nullable: false),
                    ActiveStatus = table.Column<bool>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TransactionById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherExpertiesTransactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherExperties_CreatedById",
                table: "TeacherExperties",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherExperties_SchoolBranchId",
                table: "TeacherExperties",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherExperties_SubjectId",
                table: "TeacherExperties",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherExperties_TeacherId",
                table: "TeacherExperties",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherExperties");

            migrationBuilder.DropTable(
                name: "TeacherExpertiesTransactions");
        }
    }
}

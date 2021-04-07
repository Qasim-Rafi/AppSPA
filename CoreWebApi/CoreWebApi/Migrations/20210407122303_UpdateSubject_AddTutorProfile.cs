using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSubject_AddTutorProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpertRank",
                table: "Subjects");

            migrationBuilder.AddColumn<int>(
                name: "ExpertRate",
                table: "Subjects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TutorProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(nullable: false),
                    GradeLevels = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true),
                    Education = table.Column<string>(nullable: true),
                    WorkHistory = table.Column<string>(nullable: true),
                    WorkExperience = table.Column<string>(nullable: true),
                    AreasToTeach = table.Column<string>(nullable: true),
                    LanguageFluencyRate = table.Column<int>(nullable: false),
                    CommunicationSkillRate = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorProfiles_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TutorProfiles_SchoolBranchId",
                table: "TutorProfiles",
                column: "SchoolBranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TutorProfiles");

            migrationBuilder.DropColumn(
                name: "ExpertRate",
                table: "Subjects");

            migrationBuilder.AddColumn<int>(
                name: "ExpertRank",
                table: "Subjects",
                type: "int",
                nullable: true);
        }
    }
}

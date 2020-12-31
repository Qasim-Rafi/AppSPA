using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddSubstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Substitutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSectionId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<int>(nullable: true),
                    SubjectId = table.Column<int>(nullable: false),
                    SubstituteTeacherId = table.Column<int>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    SubstitutedTeacherId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substitutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Substitutions_ClassSections_ClassSectionId",
                        column: x => x.ClassSectionId,
                        principalTable: "ClassSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Substitutions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Substitutions_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Substitutions_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Substitutions_Users_SubstitutedTeacherId",
                        column: x => x.SubstitutedTeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Substitutions_ClassSectionId",
                table: "Substitutions",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutions_CreatedById",
                table: "Substitutions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutions_SchoolBranchId",
                table: "Substitutions",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutions_SubjectId",
                table: "Substitutions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutions_SubstitutedTeacherId",
                table: "Substitutions",
                column: "SubstitutedTeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Substitutions");
        }
    }
}

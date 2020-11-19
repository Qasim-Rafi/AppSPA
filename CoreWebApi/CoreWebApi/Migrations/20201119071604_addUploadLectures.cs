using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addUploadLectures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIncharge",
                table: "ClassSectionUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UploadedLectures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(nullable: false),
                    ClassSectionId = table.Column<int>(nullable: false),
                    LectureUrl = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedLectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadedLectures_ClassSections_ClassSectionId",
                        column: x => x.ClassSectionId,
                        principalTable: "ClassSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UploadedLectures_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadedLectures_ClassSectionId",
                table: "UploadedLectures",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedLectures_TeacherId",
                table: "UploadedLectures",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadedLectures");

            migrationBuilder.DropColumn(
                name: "IsIncharge",
                table: "ClassSectionUsers");
        }
    }
}

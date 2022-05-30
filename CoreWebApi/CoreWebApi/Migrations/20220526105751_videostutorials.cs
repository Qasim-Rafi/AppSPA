using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class videostutorials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideosTutorials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoUrl = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    updatebyId = table.Column<int>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    FileName = table.Column<string>(maxLength: 30, nullable: true),
                    FolderName = table.Column<string>(maxLength: 30, nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    FullPath = table.Column<string>(nullable: true),
                    ClassId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideosTutorials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideosTutorials_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideosTutorials_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideosTutorials_ClassId",
                table: "VideosTutorials",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_VideosTutorials_SectionId",
                table: "VideosTutorials",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideosTutorials");
        }
    }
}

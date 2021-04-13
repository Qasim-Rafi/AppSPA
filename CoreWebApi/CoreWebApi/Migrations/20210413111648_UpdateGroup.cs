using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClassSectionId",
                table: "Groups",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "GroupLectureTime",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TutorClassName",
                table: "Groups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupLectureTime",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TutorClassName",
                table: "Groups");

            migrationBuilder.AlterColumn<int>(
                name: "ClassSectionId",
                table: "Groups",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

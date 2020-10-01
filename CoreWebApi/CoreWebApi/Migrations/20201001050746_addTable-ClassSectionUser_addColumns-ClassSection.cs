using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addTableClassSectionUser_addColumnsClassSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves");

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionUserId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionId",
                table: "Sections",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionUserId",
                table: "ClassSections",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "ClassSections",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ClassSections",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionId",
                table: "Class",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClassSectionUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSectionId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSectionUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClassSectionUserId",
                table: "Users",
                column: "ClassSectionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_ClassSectionId",
                table: "Sections",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSections_ClassSectionUserId",
                table: "ClassSections",
                column: "ClassSectionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Class_ClassSectionId",
                table: "Class",
                column: "ClassSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Class_ClassSections_ClassSectionId",
                table: "Class",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_ClassSectionUsers_ClassSectionUserId",
                table: "ClassSections",
                column: "ClassSectionUserId",
                principalTable: "ClassSectionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_ClassSections_ClassSectionId",
                table: "Sections",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ClassSectionUsers_ClassSectionUserId",
                table: "Users",
                column: "ClassSectionUserId",
                principalTable: "ClassSectionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Class_ClassSections_ClassSectionId",
                table: "Class");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_ClassSectionUsers_ClassSectionUserId",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_ClassSections_ClassSectionId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ClassSectionUsers_ClassSectionUserId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ClassSectionUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_ClassSectionUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Sections_ClassSectionId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_ClassSections_ClassSectionUserId",
                table: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_Class_ClassSectionId",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "ClassSectionUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "ClassSectionUserId",
                table: "ClassSections");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ClassSections");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ClassSections");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "Class");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves",
                column: "LeaveTypeId",
                unique: true);
        }
    }
}

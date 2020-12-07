using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addCSTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ClassSectionUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolBranchId",
                table: "ClassSectionUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClassSectionTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSectionId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSectionTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSectionTransactions_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionUsers_SchoolBranchId",
                table: "ClassSectionUsers",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSectionTransactions_DeletedById",
                table: "ClassSectionTransactions",
                column: "DeletedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionUsers_SchoolBranch_SchoolBranchId",
                table: "ClassSectionUsers",
                column: "SchoolBranchId",
                principalTable: "SchoolBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionUsers_SchoolBranch_SchoolBranchId",
                table: "ClassSectionUsers");

            migrationBuilder.DropTable(
                name: "ClassSectionTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ClassSectionUsers_SchoolBranchId",
                table: "ClassSectionUsers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ClassSectionUsers");

            migrationBuilder.DropColumn(
                name: "SchoolBranchId",
                table: "ClassSectionUsers");
        }
    }
}

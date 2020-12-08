using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addUnMapUserTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnMapUserTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSectionId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UnMappedDate = table.Column<DateTime>(nullable: true),
                    UnMappedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnMapUserTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnMapUserTransactions_Users_UnMappedById",
                        column: x => x.UnMappedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnMapUserTransactions_UnMappedById",
                table: "UnMapUserTransactions",
                column: "UnMappedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnMapUserTransactions");
        }
    }
}

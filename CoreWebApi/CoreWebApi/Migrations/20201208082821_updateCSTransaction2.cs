using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class updateCSTransaction2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnMapUserTransactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "MappedCreationDate",
                table: "ClassSectionTransactions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "ClassSectionTransactions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MappedCreationDate",
                table: "ClassSectionTransactions");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "ClassSectionTransactions");

            migrationBuilder.CreateTable(
                name: "UnMapUserTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassSectionId = table.Column<int>(type: "int", nullable: false),
                    UnMappedById = table.Column<int>(type: "int", nullable: false),
                    UnMappedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
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
    }
}

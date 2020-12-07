using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class updateCSTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ClassSectionTransactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "ClassSectionTransactions",
                nullable: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "ClassSectionTransactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ClassSectionTransactions",
                type: "datetime2",
                nullable: true);
        }
    }
}

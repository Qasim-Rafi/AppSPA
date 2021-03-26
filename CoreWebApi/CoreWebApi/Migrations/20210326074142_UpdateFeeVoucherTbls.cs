using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateFeeVoucherTbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillDate",
                table: "FeeVoucherRecords");

            migrationBuilder.AddColumn<int>(
                name: "AnnualOrSemesterId",
                table: "FeeVoucherRecords",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BillGenerationDate",
                table: "FeeVoucherRecords",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualOrSemesterId",
                table: "FeeVoucherRecords");

            migrationBuilder.DropColumn(
                name: "BillGenerationDate",
                table: "FeeVoucherRecords");

            migrationBuilder.AddColumn<DateTime>(
                name: "BillDate",
                table: "FeeVoucherRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateVoucherRecords2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeVoucherRecords_FeeVoucherDetails_VoucherDetailId",
                table: "FeeVoucherRecords");

            migrationBuilder.DropIndex(
                name: "IX_FeeVoucherRecords_VoucherDetailId",
                table: "FeeVoucherRecords");

            migrationBuilder.DropColumn(
                name: "VoucherDetailId",
                table: "FeeVoucherRecords");

            migrationBuilder.AddColumn<int>(
                name: "BankAccountId",
                table: "FeeVoucherRecords",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VoucherDetailIds",
                table: "FeeVoucherRecords",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "FeeVoucherRecords");

            migrationBuilder.DropColumn(
                name: "VoucherDetailIds",
                table: "FeeVoucherRecords");

            migrationBuilder.AddColumn<int>(
                name: "VoucherDetailId",
                table: "FeeVoucherRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FeeVoucherRecords_VoucherDetailId",
                table: "FeeVoucherRecords",
                column: "VoucherDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeVoucherRecords_FeeVoucherDetails_VoucherDetailId",
                table: "FeeVoucherRecords",
                column: "VoucherDetailId",
                principalTable: "FeeVoucherDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

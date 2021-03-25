using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateVoucherRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcessionId",
                table: "FeeVoucherRecords");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "SemesterFeeMappings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcessionDetails",
                table: "FeeVoucherRecords",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeVoucherRecords_FeeVoucherDetails_VoucherDetailId",
                table: "FeeVoucherRecords");

            migrationBuilder.DropIndex(
                name: "IX_FeeVoucherRecords_VoucherDetailId",
                table: "FeeVoucherRecords");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "SemesterFeeMappings");

            migrationBuilder.DropColumn(
                name: "ConcessionDetails",
                table: "FeeVoucherRecords");

            migrationBuilder.AddColumn<int>(
                name: "ConcessionId",
                table: "FeeVoucherRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

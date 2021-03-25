using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateVoucherDetail_AddBankAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "FeeVoucherDetails");

            migrationBuilder.DropColumn(
                name: "BankAddress",
                table: "FeeVoucherDetails");

            migrationBuilder.DropColumn(
                name: "BankDetails",
                table: "FeeVoucherDetails");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "FeeVoucherDetails");

            migrationBuilder.DropColumn(
                name: "PaymentTerms",
                table: "FeeVoucherDetails");

            migrationBuilder.AddColumn<int>(
                name: "BankAccountId",
                table: "FeeVoucherDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ExtraChargesAmount",
                table: "FeeVoucherDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ExtraChargesDetails",
                table: "FeeVoucherDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "FeeVoucherDetails",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(nullable: true),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    BankAddress = table.Column<string>(nullable: true),
                    BankDetails = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccounts_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeVoucherDetails_BankAccountId",
                table: "FeeVoucherDetails",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CreatedById",
                table: "BankAccounts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_SchoolBranchId",
                table: "BankAccounts",
                column: "SchoolBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeVoucherDetails_BankAccounts_BankAccountId",
                table: "FeeVoucherDetails",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeVoucherDetails_BankAccounts_BankAccountId",
                table: "FeeVoucherDetails");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_FeeVoucherDetails_BankAccountId",
                table: "FeeVoucherDetails");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "FeeVoucherDetails");

            migrationBuilder.DropColumn(
                name: "ExtraChargesAmount",
                table: "FeeVoucherDetails");

            migrationBuilder.DropColumn(
                name: "ExtraChargesDetails",
                table: "FeeVoucherDetails");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "FeeVoucherDetails");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "FeeVoucherDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAddress",
                table: "FeeVoucherDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankDetails",
                table: "FeeVoucherDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "FeeVoucherDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTerms",
                table: "FeeVoucherDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

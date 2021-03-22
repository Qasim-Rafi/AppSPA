using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSemesterFeeMapping_AddFeeVoucherModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SemesterId",
                table: "SemesterFeeMappings",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "SemesterFeeMappings",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "SemesterFeeMappings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FeeVoucherDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(nullable: true),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    BankAddress = table.Column<string>(nullable: true),
                    BankDetails = table.Column<string>(nullable: true),
                    PaymentTerms = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeVoucherDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeVoucherDetails_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeVoucherDetails_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeVoucherRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherDetailId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    RegistrationNo = table.Column<string>(nullable: true),
                    BillNumber = table.Column<string>(nullable: true),
                    BillDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    BillMonth = table.Column<string>(nullable: true),
                    ClassSectionId = table.Column<int>(nullable: false),
                    ConcessionId = table.Column<int>(nullable: false),
                    FeeAmount = table.Column<double>(nullable: false),
                    MiscellaneousCharges = table.Column<double>(nullable: false),
                    TotalFee = table.Column<double>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeVoucherRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeVoucherRecords_ClassSections_ClassSectionId",
                        column: x => x.ClassSectionId,
                        principalTable: "ClassSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeVoucherRecords_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeVoucherRecords_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeVoucherRecords_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeVoucherDetails_CreatedById",
                table: "FeeVoucherDetails",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVoucherDetails_SchoolBranchId",
                table: "FeeVoucherDetails",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVoucherRecords_ClassSectionId",
                table: "FeeVoucherRecords",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVoucherRecords_CreatedById",
                table: "FeeVoucherRecords",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVoucherRecords_SchoolBranchId",
                table: "FeeVoucherRecords",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVoucherRecords_StudentId",
                table: "FeeVoucherRecords",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeVoucherDetails");

            migrationBuilder.DropTable(
                name: "FeeVoucherRecords");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "SemesterFeeMappings");

            migrationBuilder.AlterColumn<int>(
                name: "SemesterId",
                table: "SemesterFeeMappings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "SemesterFeeMappings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

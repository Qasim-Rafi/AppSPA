using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddFeeInstallment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeInstallments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterFeeMappingId = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    PaidMonth = table.Column<string>(nullable: true),
                    Paid = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeInstallments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeInstallments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeInstallments_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeInstallments_SemesterFeeMappings_SemesterFeeMappingId",
                        column: x => x.SemesterFeeMappingId,
                        principalTable: "SemesterFeeMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeInstallments_CreatedById",
                table: "FeeInstallments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FeeInstallments_SchoolBranchId",
                table: "FeeInstallments",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeInstallments_SemesterFeeMappingId",
                table: "FeeInstallments",
                column: "SemesterFeeMappingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeInstallments");
        }
    }
}

﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddSemesterFeeModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SemesterFeeMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(nullable: false),
                    SemesterId = table.Column<int>(nullable: false),
                    DiscountInPercentage = table.Column<int>(nullable: false),
                    FeeAfterDiscount = table.Column<double>(nullable: false),
                    InstallmentAmount = table.Column<double>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Posted = table.Column<bool>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterFeeMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemesterFeeMappings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SemesterFeeMappings_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SemesterFeeTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterId = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<int>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterFeeTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemesterFeeTransactions_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SemesterFeeTransactions_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    FeeAmount = table.Column<double>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    LateFeePlentyAmount = table.Column<int>(nullable: false),
                    LateFeeValidityInDays = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Posted = table.Column<bool>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Semesters_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Semesters_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SemesterFeeMappings_CreatedById",
                table: "SemesterFeeMappings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterFeeMappings_SchoolBranchId",
                table: "SemesterFeeMappings",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterFeeTransactions_SchoolBranchId",
                table: "SemesterFeeTransactions",
                column: "SchoolBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterFeeTransactions_UpdatedById",
                table: "SemesterFeeTransactions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_CreatedById",
                table: "Semesters",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_SchoolBranchId",
                table: "Semesters",
                column: "SchoolBranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SemesterFeeMappings");

            migrationBuilder.DropTable(
                name: "SemesterFeeTransactions");

            migrationBuilder.DropTable(
                name: "Semesters");
        }
    }
}

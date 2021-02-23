using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddRequisition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requisitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestById = table.Column<int>(nullable: false),
                    RequestDateTime = table.Column<DateTime>(nullable: false),
                    RequestComment = table.Column<string>(nullable: true),
                    ApproveById = table.Column<int>(nullable: true),
                    ApproveDateTime = table.Column<DateTime>(nullable: true),
                    ApproveComment = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SchoolBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requisitions_Users_ApproveById",
                        column: x => x.ApproveById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requisitions_Users_RequestById",
                        column: x => x.RequestById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requisitions_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_ApproveById",
                table: "Requisitions",
                column: "ApproveById");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_RequestById",
                table: "Requisitions",
                column: "RequestById");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_SchoolBranchId",
                table: "Requisitions",
                column: "SchoolBranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requisitions");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateLeave2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_LeaveApprovalType_LeaveApprovalTypeId",
                table: "Leaves");

            migrationBuilder.DropTable(
                name: "LeaveApprovalType");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_LeaveApprovalTypeId",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "LeaveApprovalTypeId",
                table: "Leaves");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Leaves",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Leaves");

            migrationBuilder.AddColumn<int>(
                name: "LeaveApprovalTypeId",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LeaveApprovalType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveApprovalType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveApprovalTypeId",
                table: "Leaves",
                column: "LeaveApprovalTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_LeaveApprovalType_LeaveApprovalTypeId",
                table: "Leaves",
                column: "LeaveApprovalTypeId",
                principalTable: "LeaveApprovalType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

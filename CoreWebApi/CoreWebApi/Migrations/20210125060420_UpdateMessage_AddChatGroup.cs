using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateMessage_AddChatGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_MessageToUserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageToUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageToUserId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "MessageToUserIds",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(nullable: true),
                    UserIds = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatGroups_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroups_CreatedById",
                table: "ChatGroups",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatGroups");

            migrationBuilder.DropColumn(
                name: "MessageToUserIds",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "MessageToUserId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageToUserId",
                table: "Messages",
                column: "MessageToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_MessageToUserId",
                table: "Messages",
                column: "MessageToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

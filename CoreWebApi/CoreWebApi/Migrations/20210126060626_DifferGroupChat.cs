using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class DifferGroupChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageToUserIds",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "MessageToUserId",
                table: "Messages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageToUserIds = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    MessageFromUserId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    MessageReplyId = table.Column<int>(nullable: true),
                    Attachment = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMessages_Users_MessageFromUserId",
                        column: x => x.MessageFromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageToUserId",
                table: "Messages",
                column: "MessageToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_MessageFromUserId",
                table: "GroupMessages",
                column: "MessageFromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_MessageToUserId",
                table: "Messages",
                column: "MessageToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_MessageToUserId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "GroupMessages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageToUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageToUserId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "MessageToUserIds",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddReply_UpdateMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReplyMessageId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Messages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MessageReplyId",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageReplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(nullable: false),
                    ReplyToUserId = table.Column<int>(nullable: false),
                    Reply = table.Column<string>(nullable: true),
                    ReplyFromUserId = table.Column<int>(nullable: false),
                    Attachment = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageReplies_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageReplies_Users_ReplyFromUserId",
                        column: x => x.ReplyFromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageReplies_Users_ReplyToUserId",
                        column: x => x.ReplyToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageFromUserId",
                table: "Messages",
                column: "MessageFromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReplies_MessageId",
                table: "MessageReplies",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReplies_ReplyFromUserId",
                table: "MessageReplies",
                column: "ReplyFromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReplies_ReplyToUserId",
                table: "MessageReplies",
                column: "ReplyToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_MessageFromUserId",
                table: "Messages",
                column: "MessageFromUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_MessageFromUserId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageReplies");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageFromUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageReplyId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReplyMessageId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

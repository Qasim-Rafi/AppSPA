using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class TblCatMessageAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessageAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(nullable: false),
                    AttachmentPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessageAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessageAttachments_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessageAttachments_MessageId",
                table: "ChatMessageAttachments",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessageAttachments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ChatMessageAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "ChatMessageAttachments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ChatMessageAttachments");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "ChatMessageAttachments");
        }
    }
}

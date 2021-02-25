using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Keyword",
                table: "UsefulResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceType",
                table: "UsefulResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "UsefulResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "UsefulResources",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keyword",
                table: "UsefulResources");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "UsefulResources");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "UsefulResources");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "UsefulResources");
        }
    }
}

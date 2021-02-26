using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSubjectContent3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectContentDetails_SubjectContents_SubjectContentId",
                table: "SubjectContentDetails");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "SubjectContentDetails",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectContentDetails_SubjectContents_SubjectContentId",
                table: "SubjectContentDetails",
                column: "SubjectContentId",
                principalTable: "SubjectContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectContentDetails_SubjectContents_SubjectContentId",
                table: "SubjectContentDetails");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "SubjectContentDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectContentDetails_SubjectContents_SubjectContentId",
                table: "SubjectContentDetails",
                column: "SubjectContentId",
                principalTable: "SubjectContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

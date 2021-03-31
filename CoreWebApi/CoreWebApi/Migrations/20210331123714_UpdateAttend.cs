using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateAttend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Attendances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassSectionId",
                table: "Attendances",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SubjectId",
                table: "Attendances",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_ClassSections_ClassSectionId",
                table: "Attendances",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Subjects_SubjectId",
                table: "Attendances",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ClassSections_ClassSectionId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Subjects_SubjectId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassSectionId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_SubjectId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Attendances");
        }
    }
}

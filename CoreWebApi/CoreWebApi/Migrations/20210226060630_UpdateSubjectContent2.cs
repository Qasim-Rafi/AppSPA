using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSubjectContent2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectContents_SubjectAssignments_SubjectAssignmentId",
                table: "SubjectContents");

            migrationBuilder.DropIndex(
                name: "IX_SubjectContents_SubjectAssignmentId",
                table: "SubjectContents");

            migrationBuilder.DropColumn(
                name: "SubjectAssignmentId",
                table: "SubjectContents");

            migrationBuilder.DropColumn(
                name: "TableOfContent",
                table: "SubjectAssignments");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "SubjectContents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "SubjectContents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectContents_ClassId",
                table: "SubjectContents",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectContents_SubjectId",
                table: "SubjectContents",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectContents_Class_ClassId",
                table: "SubjectContents",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectContents_Subjects_SubjectId",
                table: "SubjectContents",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectContents_Class_ClassId",
                table: "SubjectContents");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectContents_Subjects_SubjectId",
                table: "SubjectContents");

            migrationBuilder.DropIndex(
                name: "IX_SubjectContents_ClassId",
                table: "SubjectContents");

            migrationBuilder.DropIndex(
                name: "IX_SubjectContents_SubjectId",
                table: "SubjectContents");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "SubjectContents");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "SubjectContents");

            migrationBuilder.AddColumn<int>(
                name: "SubjectAssignmentId",
                table: "SubjectContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TableOfContent",
                table: "SubjectAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectContents_SubjectAssignmentId",
                table: "SubjectContents",
                column: "SubjectAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectContents_SubjectAssignments_SubjectAssignmentId",
                table: "SubjectContents",
                column: "SubjectAssignmentId",
                principalTable: "SubjectAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class addTableclassSection_addColumnActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Class",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionId",
                table: "Attendances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClassSections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassSectionId",
                table: "Attendances",
                column: "ClassSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_ClassSections_ClassSectionId",
                table: "Attendances",
                column: "ClassSectionId",
                principalTable: "ClassSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ClassSections_ClassSectionId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "ClassSections");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassSectionId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "ClassSectionId",
                table: "Attendances");
        }
    }
}

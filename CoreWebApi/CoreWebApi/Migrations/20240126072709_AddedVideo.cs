using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddedVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SchoolBranchId = table.Column<int>(nullable: false),
                    ClassSectionId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    TumbNail = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_ClassSections_ClassSectionId",
                        column: x => x.ClassSectionId,
                        principalTable: "ClassSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Videos_SchoolBranch_SchoolBranchId",
                        column: x => x.SchoolBranchId,
                        principalTable: "SchoolBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_ClassSectionId",
                table: "Videos",
                column: "ClassSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_SchoolBranchId",
                table: "Videos",
                column: "SchoolBranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Videos");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class AddCity_UpdateAttendnce : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ClassSections_ClassSectionId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassSectionId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ClassSectionUserAssignmentId",
                table: "Attendances");

            migrationBuilder.AlterColumn<int>(
                name: "ClassSectionId",
                table: "Attendances",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.AlterColumn<int>(
                name: "ClassSectionId",
                table: "Attendances",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassSectionUserAssignmentId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}

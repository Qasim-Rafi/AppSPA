using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateSubstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Substitutions_Users_SubstitutedTeacherId",
                table: "Substitutions");

            migrationBuilder.DropIndex(
                name: "IX_Substitutions_SubstitutedTeacherId",
                table: "Substitutions");

            migrationBuilder.DropColumn(
                name: "SubstitutedTeacherId",
                table: "Substitutions");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutions_SubstituteTeacherId",
                table: "Substitutions",
                column: "SubstituteTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Substitutions_Users_SubstituteTeacherId",
                table: "Substitutions",
                column: "SubstituteTeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Substitutions_Users_SubstituteTeacherId",
                table: "Substitutions");

            migrationBuilder.DropIndex(
                name: "IX_Substitutions_SubstituteTeacherId",
                table: "Substitutions");

            migrationBuilder.AddColumn<int>(
                name: "SubstitutedTeacherId",
                table: "Substitutions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Substitutions_SubstitutedTeacherId",
                table: "Substitutions",
                column: "SubstitutedTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Substitutions_Users_SubstitutedTeacherId",
                table: "Substitutions",
                column: "SubstitutedTeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

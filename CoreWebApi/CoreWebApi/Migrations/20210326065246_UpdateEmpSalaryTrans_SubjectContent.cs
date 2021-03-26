using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class UpdateEmpSalaryTrans_SubjectContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "SubjectContents",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "SubjectContents",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Posted",
                table: "EmployeeSalaryTransactions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectContents_SemesterId",
                table: "SubjectContents",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectContents_Semesters_SemesterId",
                table: "SubjectContents",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectContents_Semesters_SemesterId",
                table: "SubjectContents");

            migrationBuilder.DropIndex(
                name: "IX_SubjectContents_SemesterId",
                table: "SubjectContents");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "SubjectContents");

            migrationBuilder.DropColumn(
                name: "Posted",
                table: "EmployeeSalaryTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "SubjectContents",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class updateEvent4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventDaysAssignments_Events_EventId",
                table: "EventDaysAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_EventDaysAssignments_Events_EventId",
                table: "EventDaysAssignments",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventDaysAssignments_Events_EventId",
                table: "EventDaysAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_EventDaysAssignments_Events_EventId",
                table: "EventDaysAssignments",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace EventuresApp.Data.Migrations
{
    public partial class ticketCountAddedToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketCount",
                table: "Orders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketCount",
                table: "Orders");
        }
    }
}

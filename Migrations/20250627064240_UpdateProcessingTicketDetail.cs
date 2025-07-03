using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeafoodApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProcessingTicketDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutputProductName",
                table: "ProcessingTicketDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OutputQuantity",
                table: "ProcessingTicketDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "OutputSize",
                table: "ProcessingTicketDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutputProductName",
                table: "ProcessingTicketDetails");

            migrationBuilder.DropColumn(
                name: "OutputQuantity",
                table: "ProcessingTicketDetails");

            migrationBuilder.DropColumn(
                name: "OutputSize",
                table: "ProcessingTicketDetails");
        }
    }
}

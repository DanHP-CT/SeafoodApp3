using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeafoodApp.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessingTicketEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessingTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcessingTicketNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ProductionOrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcessingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessingTickets_ProductionOrders_ProductionOrderId",
                        column: x => x.ProductionOrderId,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingTicketDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcessingTicketId = table.Column<int>(type: "INTEGER", nullable: false),
                    AllocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    ProcessedQuantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingTicketDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessingTicketDetails_Allocations_AllocationId",
                        column: x => x.AllocationId,
                        principalTable: "Allocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessingTicketDetails_ProcessingTickets_ProcessingTicketId",
                        column: x => x.ProcessingTicketId,
                        principalTable: "ProcessingTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingTicketDetails_AllocationId",
                table: "ProcessingTicketDetails",
                column: "AllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingTicketDetails_ProcessingTicketId",
                table: "ProcessingTicketDetails",
                column: "ProcessingTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingTickets_ProductionOrderId",
                table: "ProcessingTickets",
                column: "ProductionOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessingTicketDetails");

            migrationBuilder.DropTable(
                name: "ProcessingTickets");
        }
    }
}

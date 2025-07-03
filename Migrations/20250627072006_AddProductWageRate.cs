using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SeafoodApp.Migrations
{
    public partial class AddProductWageRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessingStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StageName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    InputMaterial = table.Column<string>(type: "TEXT", nullable: false),
                    OutputProduct = table.Column<string>(type: "TEXT", nullable: false),
                    StandardRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingStages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductWageRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcessingStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    WageRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWageRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductWageRates_ProcessingStages_ProcessingStageId",
                        column: x => x.ProcessingStageId,
                        principalTable: "ProcessingStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "ProcessingStageId",
                table: "ProcessingTickets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductWageRates_ProcessingStageId",
                table: "ProductWageRates",
                column: "ProcessingStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessingTickets_ProcessingStages_ProcessingStageId",
                table: "ProcessingTickets",
                column: "ProcessingStageId",
                principalTable: "ProcessingStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessingTickets_ProcessingStages_ProcessingStageId",
                table: "ProcessingTickets");

            migrationBuilder.DropTable(
                name: "ProductWageRates");

            migrationBuilder.DropTable(
                name: "ProcessingStages");

            migrationBuilder.DropColumn(
                name: "ProcessingStageId",
                table: "ProcessingTickets");
        }
    }
}
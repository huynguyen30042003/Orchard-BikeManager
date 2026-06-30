using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeManagerV3.Warranty.Migrations
{
    /// <inheritdoc />
    public partial class WarrantyDbTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "warranties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warranties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "warranty_claims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarrantyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepairOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warranty_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_warranty_claims_warranties_WarrantyId",
                        column: x => x.WarrantyId,
                        principalTable: "warranties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_warranty_claims_WarrantyId",
                table: "warranty_claims",
                column: "WarrantyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "warranty_claims");

            migrationBuilder.DropTable(
                name: "warranties");
        }
    }
}

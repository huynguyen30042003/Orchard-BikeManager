using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeManagerV3.Customer.Migrations
{
    /// <inheritdoc />
    public partial class CustomerDbTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "customer_statistics",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalOrders = table.Column<int>(type: "int", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRepairs = table.Column<int>(type: "int", nullable: false),
                    LastPurchaseAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_statistics", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_customer_statistics_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PlateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FrameNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EngineNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BatterySerial = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customer_vehicles_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_ownerships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnershipStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnershipEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrentOwner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_ownerships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vehicle_ownerships_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customer_vehicles_CustomerId",
                table: "customer_vehicles",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_ownerships_CustomerId",
                table: "vehicle_ownerships",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_statistics");

            migrationBuilder.DropTable(
                name: "customer_vehicles");

            migrationBuilder.DropTable(
                name: "vehicle_ownerships");

            migrationBuilder.DropTable(
                name: "customers");
        }
    }
}

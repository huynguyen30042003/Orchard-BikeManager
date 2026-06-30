using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeManagerV3.Order.Migrations
{
    /// <inheritdoc />
    public partial class OrderV2Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "order_items",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProfitAmount",
                table: "order_items",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "ProfitAmount",
                table: "order_items");
        }
    }
}

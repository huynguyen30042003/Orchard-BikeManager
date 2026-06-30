using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeManagerV3.Suppliers.Migrations
{
    /// <inheritdoc />
    public partial class SuppliersV3DBTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "purchase_order_items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductVariantSKU",
                table: "purchase_order_items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "purchase_order_items");

            migrationBuilder.DropColumn(
                name: "ProductVariantSKU",
                table: "purchase_order_items");
        }
    }
}

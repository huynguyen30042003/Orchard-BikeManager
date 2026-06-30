using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeManagerV3.Suppliers.Migrations
{
    /// <inheritdoc />
    public partial class SuppliersV4DbTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrackSerial",
                table: "purchase_order_items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackSerial",
                table: "purchase_order_items");
        }
    }
}

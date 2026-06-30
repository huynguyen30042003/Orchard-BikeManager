using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeManagerV3.Product.Migrations
{
    /// <inheritdoc />
    public partial class TrackSerialDbTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "product_variants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "TrackSerial",
                table: "product_variants",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackSerial",
                table: "product_variants");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "product_variants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

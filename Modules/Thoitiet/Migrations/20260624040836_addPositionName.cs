using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thoitiet.Migrations
{
    /// <inheritdoc />
    public partial class addPositionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PositionName",
                table: "WeatherConfigurations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionName",
                table: "WeatherConfigurations");
        }
    }
}

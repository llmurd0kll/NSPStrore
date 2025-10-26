using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NspStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductPriceAndAddPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

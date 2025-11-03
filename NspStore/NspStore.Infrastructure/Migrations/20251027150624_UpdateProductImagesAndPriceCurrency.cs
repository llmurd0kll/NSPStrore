using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NspStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductImagesAndPriceCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "ProductImages",
                newName: "ThumbUrl");

            migrationBuilder.AddColumn<string>(
                name: "MediumUrl",
                table: "ProductImages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalUrl",
                table: "ProductImages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediumUrl",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "OriginalUrl",
                table: "ProductImages");

            migrationBuilder.RenameColumn(
                name: "ThumbUrl",
                table: "ProductImages",
                newName: "Url");
        }
    }
}

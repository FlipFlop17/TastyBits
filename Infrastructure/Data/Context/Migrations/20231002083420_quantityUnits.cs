using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class quantityUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity_g",
                schema: "TastySchema",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "Quantity_x",
                schema: "TastySchema",
                table: "RecipeIngredients",
                newName: "Quantity");

            migrationBuilder.AddColumn<string>(
                name: "QuantityUnit",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityUnit",
                schema: "TastySchema",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "TastySchema",
                table: "RecipeIngredients",
                newName: "Quantity_x");

            migrationBuilder.AddColumn<double>(
                name: "Quantity_g",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

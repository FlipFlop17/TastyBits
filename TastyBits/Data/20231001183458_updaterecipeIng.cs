using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TastyBits.Data
{
    /// <inheritdoc />
    public partial class updaterecipeIng : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Quantity_x",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "bigint");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity_g",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Quantity_x",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "text",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Quantity_g",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "text",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TastyBits.Data
{
    /// <inheritdoc />
    public partial class updateMealsNulls2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mililiters",
                schema: "TastySchema",
                table: "RecipeIngredients");

            migrationBuilder.AlterColumn<string>(
                name: "ImageData",
                schema: "TastySchema",
                table: "RecipeImage",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mililiters",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ImageData",
                schema: "TastySchema",
                table: "RecipeImage",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}

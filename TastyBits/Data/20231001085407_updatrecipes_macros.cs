using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TastyBits.Data
{
    /// <inheritdoc />
    public partial class updatrecipes_macros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "TastySchema",
                table: "RecipeIngredients",
                newName: "Quantity_x");

            migrationBuilder.AddColumn<int>(
                name: "MealId",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mililiters",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Quantity_g",
                schema: "TastySchema",
                table: "RecipeIngredients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instrunctions",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "CaloriesPer100Gram",
                schema: "TastySchema",
                table: "Ingredients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Carbs_g",
                schema: "TastySchema",
                table: "Ingredients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fat_g",
                schema: "TastySchema",
                table: "Ingredients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fiber_g",
                schema: "TastySchema",
                table: "Ingredients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Potassium_mg",
                schema: "TastySchema",
                table: "Ingredients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Proteing_g",
                schema: "TastySchema",
                table: "Ingredients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Sodium_mg",
                schema: "TastySchema",
                table: "Ingredients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Sugar_g",
                schema: "TastySchema",
                table: "Ingredients",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_MealId",
                schema: "TastySchema",
                table: "RecipeIngredients",
                column: "MealId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Meals_MealId",
                schema: "TastySchema",
                table: "RecipeIngredients",
                column: "MealId",
                principalSchema: "TastySchema",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Meals_MealId",
                schema: "TastySchema",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_MealId",
                schema: "TastySchema",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "MealId",
                schema: "TastySchema",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "Mililiters",
                schema: "TastySchema",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "Quantity_g",
                schema: "TastySchema",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "Instrunctions",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "CaloriesPer100Gram",
                schema: "TastySchema",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Carbs_g",
                schema: "TastySchema",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Fat_g",
                schema: "TastySchema",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Fiber_g",
                schema: "TastySchema",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Potassium_mg",
                schema: "TastySchema",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Proteing_g",
                schema: "TastySchema",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Sodium_mg",
                schema: "TastySchema",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Sugar_g",
                schema: "TastySchema",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "Quantity_x",
                schema: "TastySchema",
                table: "RecipeIngredients",
                newName: "Quantity");
        }
    }
}

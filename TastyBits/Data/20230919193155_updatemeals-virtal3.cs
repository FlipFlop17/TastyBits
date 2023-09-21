using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TastyBits.Data
{
    /// <inheritdoc />
    public partial class updatemealsvirtal3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_RecipeImage_RecipeImageId",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_RecipeImage_MealId",
                schema: "TastySchema",
                table: "RecipeImage");

            migrationBuilder.DropIndex(
                name: "IX_Meals_RecipeImageId",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "RecipeImageId",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImage_MealId",
                schema: "TastySchema",
                table: "RecipeImage",
                column: "MealId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeImage_MealId",
                schema: "TastySchema",
                table: "RecipeImage");

            migrationBuilder.AddColumn<int>(
                name: "RecipeImageId",
                schema: "TastySchema",
                table: "Meals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImage_MealId",
                schema: "TastySchema",
                table: "RecipeImage",
                column: "MealId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meals_RecipeImageId",
                schema: "TastySchema",
                table: "Meals",
                column: "RecipeImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_RecipeImage_RecipeImageId",
                schema: "TastySchema",
                table: "Meals",
                column: "RecipeImageId",
                principalSchema: "TastySchema",
                principalTable: "RecipeImage",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

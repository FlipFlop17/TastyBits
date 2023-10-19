using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class updatemealsvirtal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeImage_MealId",
                schema: "TastySchema",
                table: "RecipeImage");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImage_MealId",
                schema: "TastySchema",
                table: "RecipeImage",
                column: "MealId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeImage_MealId",
                schema: "TastySchema",
                table: "RecipeImage");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImage_MealId",
                schema: "TastySchema",
                table: "RecipeImage",
                column: "MealId");
        }
    }
}

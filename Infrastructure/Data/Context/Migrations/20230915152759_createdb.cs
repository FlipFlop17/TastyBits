using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class createdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TastySchema");

            migrationBuilder.CreateTable(
                name: "Ingredients",
                schema: "TastySchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                schema: "TastySchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PrepTime = table.Column<int>(type: "integer", nullable: false),
                    CookingTime = table.Column<int>(type: "integer", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "TastySchema",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                schema: "TastySchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IngredientId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalSchema: "TastySchema",
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeImage",
                schema: "TastySchema",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MealId = table.Column<int>(type: "integer", nullable: false),
                    ImageData = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeImage", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_RecipeImage_Meals_MealId",
                        column: x => x.MealId,
                        principalSchema: "TastySchema",
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meals_UserId",
                schema: "TastySchema",
                table: "Meals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImage_MealId",
                schema: "TastySchema",
                table: "RecipeImage",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                schema: "TastySchema",
                table: "RecipeIngredients",
                column: "IngredientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "RecipeImage",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "RecipeIngredients",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "Meals",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "Ingredients",
                schema: "TastySchema");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "TastySchema");
        }
    }
}

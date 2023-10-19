using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class updatemealstypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBreakfast",
                schema: "TastySchema",
                table: "Meals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDesert",
                schema: "TastySchema",
                table: "Meals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDinner",
                schema: "TastySchema",
                table: "Meals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLunch",
                schema: "TastySchema",
                table: "Meals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSnack",
                schema: "TastySchema",
                table: "Meals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVegan",
                schema: "TastySchema",
                table: "Meals",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVegetarian",
                schema: "TastySchema",
                table: "Meals",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBreakfast",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "IsDesert",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "IsDinner",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "IsLunch",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "IsSnack",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "IsVegan",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "IsVegetarian",
                schema: "TastySchema",
                table: "Meals");
        }
    }
}

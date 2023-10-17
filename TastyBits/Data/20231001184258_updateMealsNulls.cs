using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TastyBits.Data
{
    /// <inheritdoc />
    public partial class updateMealsNulls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instrunctions",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.AlterColumn<string>(
                name: "ServingsAmount",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PrepTime",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CookingTime",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instructions",
                schema: "TastySchema",
                table: "Meals");

            migrationBuilder.AlterColumn<string>(
                name: "ServingsAmount",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrepTime",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CookingTime",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instrunctions",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

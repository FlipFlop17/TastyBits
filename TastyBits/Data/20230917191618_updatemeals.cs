using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TastyBits.Data
{
    /// <inheritdoc />
    public partial class updatemeals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PrepTime",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "CookingTime",
                schema: "TastySchema",
                table: "Meals",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PrepTime",
                schema: "TastySchema",
                table: "Meals",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CookingTime",
                schema: "TastySchema",
                table: "Meals",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}

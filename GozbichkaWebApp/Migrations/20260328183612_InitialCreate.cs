using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GozbichkaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Recipes_RecipeId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeProducts_Products_ProductId",
                table: "RecipeProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Difficulties_DifficultyId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Products_RecipeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MealCategory_ID",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "difficulty",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "DifficultyId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubstituteProductId",
                table: "RecipeProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeProducts_SubstituteProductId",
                table: "RecipeProducts",
                column: "SubstituteProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeProducts_Products_ProductId",
                table: "RecipeProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeProducts_Products_SubstituteProductId",
                table: "RecipeProducts",
                column: "SubstituteProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Difficulties_DifficultyId",
                table: "Recipes",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "DifficultyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeProducts_Products_ProductId",
                table: "RecipeProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeProducts_Products_SubstituteProductId",
                table: "RecipeProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Difficulties_DifficultyId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_RecipeProducts_SubstituteProductId",
                table: "RecipeProducts");

            migrationBuilder.AlterColumn<int>(
                name: "DifficultyId",
                table: "Recipes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MealCategory_ID",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "difficulty",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SubstituteProductId",
                table: "RecipeProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_RecipeId",
                table: "Products",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Recipes_RecipeId",
                table: "Products",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeProducts_Products_ProductId",
                table: "RecipeProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Difficulties_DifficultyId",
                table: "Recipes",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "DifficultyId");
        }
    }
}

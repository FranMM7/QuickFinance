using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddComputedColumnToFinanceDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingLists_Categories_CategoryId",
                table: "ShoppingLists");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "ShoppingLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "qty",
                table: "ShoppingLists",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "ShoppingLists",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "[qty] * [Amount]");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingLists_Categories_CategoryId",
                table: "ShoppingLists",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingLists_Categories_CategoryId",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "qty",
                table: "ShoppingLists");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "ShoppingLists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingLists_Categories_CategoryId",
                table: "ShoppingLists",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class RecreateShoppingLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ShoppingLists",
                newName: "ItemName");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "ShoppingLists",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "ShoppingLists");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "ShoppingLists",
                newName: "Description");
        }
    }
}

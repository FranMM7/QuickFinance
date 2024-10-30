using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class drop_create_shoppingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the ShoppingLists table if it exists
            migrationBuilder.DropTable(name: "ShoppingLists");

            // Recreate the table with the desired schema
            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    ItemName = table.Column<string>(nullable: false),
                    Brand = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    // Define the computed column directly here
                    Subtotal = table.Column<decimal>(nullable: true, computedColumnSql: "[Quantity] * [Amount]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_Shoppings_ShoppingId",
                        column: x => x.ShoppingId,
                        principalTable: "Shoppings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Add indices if needed
            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_ShoppingId",
                table: "ShoppingLists",
                column: "ShoppingId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_CategoryId",
                table: "ShoppingLists",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_LocationId",
                table: "ShoppingLists",
                column: "LocationId");
        }
    

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

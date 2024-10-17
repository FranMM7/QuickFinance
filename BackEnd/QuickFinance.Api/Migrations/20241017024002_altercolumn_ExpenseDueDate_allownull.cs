using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class altercolumn_ExpenseDueDate_allownull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpenseDueDate",
                table: "Expenses",
                nullable: true  // Whether the column allows nulls
               
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
             name: "ExpenseDueDate",
             table: "Expenses",
             nullable: false  // Whether the column allows nulls

         );
        }
    }
}

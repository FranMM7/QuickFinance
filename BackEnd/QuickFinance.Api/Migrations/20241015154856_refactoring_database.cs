using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class refactoring_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "ShoppingLists");

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal", // New name
                table: "ShoppingLists",
                type: "decimal(18,2)", // Make sure to set the appropriate type
                nullable: true, // Set it according to your requirement
                computedColumnSql: "[qty] * [Amount]"); // Your computed logic

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PaymentMethods",
                newName: "PaymentMethodName");

            migrationBuilder.RenameColumn(
                name: "ExpenseType",
                table: "FinanceDetails",
                newName: "ExpenseCategory");

            migrationBuilder.RenameColumn(
                name: "Executed",
                table: "Expenses",
                newName: "IsExecuted");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Expenses",
                newName: "ExpenseDueDate");

            migrationBuilder.RenameColumn(
                name: "budgetlimit",
                table: "Categories",
                newName: "BudgetLimit");

            migrationBuilder.RenameColumn(
                name: "TotalBudget",
                table: "Budgets",
                newName: "TotalAllocatedBudget");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Shoppings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShoppingLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "PaymentMethods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "FinanceEvaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Shoppings");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "State",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "State",
                table: "FinanceEvaluations");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "Subtotal",
                table: "ShoppingLists",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodName",
                table: "PaymentMethods",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ExpenseCategory",
                table: "FinanceDetails",
                newName: "ExpenseType");

            migrationBuilder.RenameColumn(
                name: "IsExecuted",
                table: "Expenses",
                newName: "Executed");

            migrationBuilder.RenameColumn(
                name: "ExpenseDueDate",
                table: "Expenses",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "BudgetLimit",
                table: "Categories",
                newName: "budgetlimit");

            migrationBuilder.RenameColumn(
                name: "TotalAllocatedBudget",
                table: "Budgets",
                newName: "TotalBudget");
        }
    }
}

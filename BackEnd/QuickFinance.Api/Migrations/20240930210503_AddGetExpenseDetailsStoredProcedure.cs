using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGetExpenseDetailsStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE GetExpenseDetails
            AS
            BEGIN
                SET NOCOUNT ON;

                SELECT 
                    Expenses.Id, 
                    Expenses.Description, 
                    Expenses.Amount, 
                    Expenses.DueDate, 
                    Expenses.CategoryId, 
                    Categories.Name AS Category, 
                    Expenses.BudgetId, 
                    Budgets.Month, 
                    Budgets.TotalBudget, 
                    Expenses.PaymentMethodId, 
                    PaymentMethods.Name AS PaymentMethod,
                    Expenses.Executed, 
                    ModifiedOn = CASE 
                        WHEN Expenses.UpdatedOn IS NULL THEN Expenses.CreatedOn 
                        ELSE Expenses.UpdatedOn 
                    END 
                FROM 
                    Expenses 
                LEFT OUTER JOIN Categories 
                    ON Expenses.CategoryId = Categories.Id 
                LEFT OUTER JOIN Budgets 
                    ON Expenses.BudgetId = Budgets.Id 
                LEFT OUTER JOIN PaymentMethods 
                    ON Expenses.PaymentMethodId = PaymentMethods.Id;
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetExpenseDetails");
        }
    }
}

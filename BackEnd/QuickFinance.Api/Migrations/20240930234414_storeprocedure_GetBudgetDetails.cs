using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    public partial class storeprocedure_GetBudgetDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE GetBudgetDetails
            AS
            BEGIN
                -- =============================================
                -- Author:        Francis Edgardo Mejía Medina
                -- Create date:   [Insert Date]
                -- Description:   Retrieves budget details, including total and executed budgets for each month.
                -- =============================================
                SET NOCOUNT ON;

                SELECT 
                    Budgets.Id, 
                    Budgets.TotalBudget, 
                    SUM(CASE WHEN Expenses.Executed = 1 THEN Expenses.Amount ELSE 0 END) AS ExecutedBudget,
                    Budgets.Month,
                    CASE 
                        WHEN Budgets.UpdatedOn IS NULL THEN Budgets.CreatedOn 
                        ELSE Budgets.UpdatedOn 
                    END AS ModifiedOn
                FROM Budgets
                LEFT OUTER JOIN Expenses ON Expenses.BudgetId = Budgets.Id
                GROUP BY Budgets.Id, Budgets.TotalBudget, Budgets.Month, Budgets.CreatedOn, Budgets.UpdatedOn;
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetBudgetDetails");
        }
    }


}

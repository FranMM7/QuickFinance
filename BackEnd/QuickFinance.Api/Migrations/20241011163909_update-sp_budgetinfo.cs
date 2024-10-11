using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class updatesp_budgetinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[sp_GetBudgetOverviewJSON]
AS 
BEGIN 
    -- =============================================
    -- Author:        FRANCIS MEJIA
    -- Create date:   2024-10-7
    -- Description:   Retrives a summary of the top 5 budgets and the month with the hightest expenses 
    -- Last Update:   2024-10-11
    -- Update:        Added Budget and Expense Id
    -- =============================================
       Declare @JsonResult nvarchar(max)

       set @JsonResult= (SELECT 
        (SELECT TOP (5)
			B.Id as BudgetId,
			E.Id as ExpensesId,
            B.Month, 
            B.TotalBudget,
            SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalBudget - SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
        GROUP BY B.Id, E.Id, B.Month, B.CreatedOn, B.TotalBudget
        ORDER BY B.CreatedOn DESC
        FOR JSON PATH) AS BudgetTop5,
    
        (SELECT TOP 1 
			B.Id as BudgetId,
			E.Id as ExpensesId,
            B.Month, 
            B.TotalBudget,
            SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalBudget - SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
        WHERE YEAR(B.CreatedOn) = YEAR(GETDATE()) 
        GROUP BY B.Id, E.Id, B.Month, B.CreatedOn, B.TotalBudget
        HAVING SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) >= 0
        ORDER BY B.CreatedOn DESC
        FOR JSON PATH) AS MonthWithHighestExpenses
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)


    SELECT @JsonResult                   
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

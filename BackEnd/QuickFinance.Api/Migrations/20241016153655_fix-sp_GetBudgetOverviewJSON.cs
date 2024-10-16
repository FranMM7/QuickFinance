using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class fixsp_GetBudgetOverviewJSON : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[sp_GetBudgetOverviewJSON]
AS 
BEGIN 
    -- =============================================
    -- Author:        FRANCIS MEJIA
    -- Create date:   2024-10-7
    -- Description:   Retrives a summary of the top 5 budgets and the month with the hightest expenses 
    -- Modified On:   2024-10-16
    -- Update:        Fix second json to show the record with the Highest expenses  
    -- =============================================
    Declare @JsonResult nvarchar(max)

    set @JsonResult= (SELECT 
        (SELECT TOP (5)
			B.Id as BudgetId,
            B.Title, 
            B.TotalAllocatedBudget AS Total,
            SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalAllocatedBudget - SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
		WHERE B.State = 1
        GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalAllocatedBudget
        ORDER BY B.CreatedOn DESC
        FOR JSON PATH) AS BudgetTop5,
    
        (SELECT TOP 1 
			B.Id as BudgetId,
            B.Title, 
            B.TotalAllocatedBudget,
            SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalAllocatedBudget - SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
        WHERE YEAR(B.CreatedOn) = YEAR(GETDATE()) 
		AND B.State = 1
        GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalAllocatedBudget
        HAVING SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) >= 0
		ORDER BY Expenses DESC
        FOR JSON PATH) AS RecordWithHighestExpenses
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

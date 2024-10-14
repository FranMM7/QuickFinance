using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class updatemultiplestps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //update store procedure GetExpenseDetails
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetExpenseDetails]
	@BudgetId int,
	@PageNumber as int = 1,
	@RowsPage int = 20
AS
BEGIN
-- =============================================
-- Author:        Francis Mejía 
-- Create date:   2024-09-30
-- Description:   Retrieves budget details, including total and executed budgets for each month.
-- Updated date:  2024-10-14
-- Lastes changes: Added the rowspage parameter, change column budgets.month to budgets.title 
-- =============================================
    SET NOCOUNT ON;

	--Declare @RowsPage int = 20

    SELECT 
        Expenses.Id, 
        Expenses.Description, 
        Expenses.Amount, 
        Expenses.DueDate, 
        Expenses.CategoryId, 
        Categories.Name AS Category, 
        Expenses.BudgetId, 
        Budgets.Title, 
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
        ON Expenses.PaymentMethodId = PaymentMethods.Id
	WHERE Budgets.Id = @BudgetId
	Order by Expenses.Id 
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END
");

            //update store procedure GetCategoryDetails
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[GetCategoryDetails]
	@PageNumber as int = 1,
	@RowsPage int = 20
AS
BEGIN
	-- =============================================
	-- Author:		Francis E Mejia
	-- Create date: 24-09-25
	-- Description:	Will display basic information about the category
	-- Updated date:  2024-10-14
	-- Lastes changes: Added Rows Page as parameter
    -- =============================================
	SET NOCOUNT ON;

	--Declare @RowsPage int = 20

    SELECT 
        c.Id,
        c.Name,
		c.BudgetLimit,
		c.CreatedOn,
		C.UpdatedOn,
        COUNT(e.Id) AS ExpenseCount,
        CAST(ISNULL(SUM(CASE WHEN e.Executed = 1 THEN e.Amount END), 0) AS decimal(18,2)) AS TotalExpended,
        ModifiedOn = CAST( CASE WHEN C.UpdatedOn IS NULL THEN C.CreatedOn ELSE C.UpdatedOn END AS DATE)
    FROM 
        Categories c
    LEFT JOIN 
        Expenses e ON e.CategoryId = c.Id
    GROUP BY 
        c.Id, c.CreatedOn, c.UpdatedOn, c.Name, c.BudgetLimit
	ORDER BY c.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END");

            //update store procedure GetBudgetDetails
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[GetBudgetDetails]
	@PageNumber as int = 1,
	@RowsPage as int = 20
AS
BEGIN
    -- =============================================
    -- Author:        Francis Edgardo Mejía Medina
    -- Create date:   24-09-25
    -- Description:   Retrieves budget details, including total and executed budgets for each month.
	-- Updated date:  2024-10-14
	-- Lastes changes: Rename budgets.month to budgets.title, added RowsPage as parameter
    -- =============================================
    SET NOCOUNT ON;

    SELECT 
        Budgets.Id, 
        Budgets.TotalBudget, 
        SUM(CASE WHEN Expenses.Executed = 1 THEN Expenses.Amount ELSE 0 END) AS ExecutedBudget,
        Budgets.Title,
        CASE 
            WHEN Budgets.UpdatedOn IS NULL THEN Budgets.CreatedOn 
            ELSE Budgets.UpdatedOn 
        END AS ModifiedOn
    FROM Budgets
    LEFT OUTER JOIN Expenses ON Expenses.BudgetId = Budgets.Id
    GROUP BY Budgets.Id, Budgets.TotalBudget, Budgets.Title, Budgets.CreatedOn, Budgets.UpdatedOn
	ORDER BY Budgets.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END");
           
            //update store procedure sp_GetBudgetOverviewJSON
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[sp_GetBudgetOverviewJSON]
AS 
BEGIN 
    -- =============================================
    -- Author:        FRANCIS MEJIA
    -- Create date:   2024-10-7
    -- Description:   Retrives a summary of the top 5 budgets and the month with the hightest expenses 
    -- Modified On:   2024-10-14
    -- Update:        column b.month was rename to b.title 
    -- =============================================
    Declare @JsonResult nvarchar(max)

    set @JsonResult= (SELECT 
        (SELECT TOP (5)
			B.Id as BudgetId,
            B.Title, 
            B.TotalBudget,
            SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalBudget - SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
        GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalBudget
        ORDER BY B.CreatedOn DESC
        FOR JSON PATH) AS BudgetTop5,
    
        (SELECT TOP 1 
			B.Id as BudgetId,
            B.Title, 
            B.TotalBudget,
            SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalBudget - SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
        WHERE YEAR(B.CreatedOn) = YEAR(GETDATE()) 
        GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalBudget
        HAVING SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) >= 0
        ORDER BY B.CreatedOn DESC
        FOR JSON PATH) AS MonthWithHighestExpenses
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)


    SELECT @JsonResult               
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

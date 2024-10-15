using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class refactoringstoreprocedures241015 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //refactoring store procudures 

            //ALTER STORE PROCEDURE GetBudgetDetails
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
        Budgets.TotalAllocatedBudget, 
        SUM(CASE WHEN Expenses.IsExecuted = 1 THEN Expenses.Amount ELSE 0 END) AS ExecutedBudget,
        Budgets.Title,
        CASE 
            WHEN Budgets.UpdatedOn IS NULL THEN Budgets.CreatedOn 
            ELSE Budgets.UpdatedOn 
        END AS ModifiedOn
    FROM Budgets
    LEFT OUTER JOIN Expenses ON Expenses.BudgetId = Budgets.Id
	WHERE Budgets.State = 1
    GROUP BY Budgets.Id, Budgets.TotalAllocatedBudget, Budgets.Title, Budgets.CreatedOn, Budgets.UpdatedOn
	ORDER BY Budgets.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END");

            //ALTER STORE PROCEDURE GetCategoryDetails
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
        CAST(ISNULL(SUM(CASE WHEN e.IsExecuted = 1 THEN e.Amount END), 0) AS decimal(18,2)) AS TotalExpended,
        ModifiedOn = CAST( CASE WHEN C.UpdatedOn IS NULL THEN C.CreatedOn ELSE C.UpdatedOn END AS DATE)
    FROM 
        Categories c
    LEFT JOIN 
        Expenses e ON e.CategoryId = c.Id
	WHERE C.State = 1
    GROUP BY 
        c.Id, c.CreatedOn, c.UpdatedOn, c.Name, c.BudgetLimit
	ORDER BY c.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END");

            //ALTER STORE PROCEDURE GetExpenseDetails
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[GetExpenseDetails]
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
        Expenses.ExpenseDueDate, 
        Expenses.CategoryId, 
        Categories.Name AS Category, 
        Expenses.BudgetId, 
        Budgets.Title, 
        Budgets.TotalAllocatedBudget, 
        Expenses.PaymentMethodId, 
        PaymentMethodName AS PaymentMethod,
        Expenses.IsExecuted, 
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

            //ALTER STORE PROCEDURE sp_GetBudgetOverviewJSON
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
        ORDER BY B.CreatedOn DESC
        FOR JSON PATH) AS MonthWithHighestExpenses
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)


    SELECT @JsonResult               
END
");

            //ALTER STORE PROCEDURE stp_getfinanceEvaluations
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[stp_getfinanceEvaluations]
	-- Add the parameters for the stored procedure here
	@PageNumber int = 1,
	@RowsPage int = 20
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- =============================================
	-- Author:		Francis Mejia
	-- Create date: 2024-10-14
	-- Description:	Finance Evaluation pagination // 1 = Important 2 = Ghost Expense 3 = Ant Expense 4 = Vampire Expense
	-- =============================================
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	FE.Id,
			FE.Title,
			c.Name as Category,
			FD.[Description], 
			CASE WHEN FD.ExpenseCategory = 1 THEN 'IMPORTANT' 
				 WHEN FD.ExpenseCategory = 2 THEN 'Ghost Expense' 
				 WHEN FD.ExpenseCategory = 3 THEN 'Ant Expense' 
				 WHEN FD.ExpenseCategory = 4 THEN 'Vampire Expense' 
			ELSE 'OTHER/ND' END ExpenseType,
			Amount
	FROM FinanceEvaluations FE
	LEFT OUTER JOIN FinanceDetails FD 
		ON FE.Id = FD.FinanceId
	LEFT OUTER JOIN Categories C
		ON FD.CategoryId = C.Id
	WHERE FE.State=1
	ORDER BY FE.Id, FD.Id DESC
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END
");

            //ALTER STORE PROCEDURE Stp_getShoppinglist
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[Stp_getShoppinglist]
	-- Add the parameters for the stored procedure here
	@PageNumber int = 1,
	@RowsPage int = 20
AS
BEGIN
-- =============================================
-- Author:		FRANCIS MEJIA
-- Create date: 2024-10-14
-- Description:	Shopping Pagination 
-- Update date: 2024-10-14
-- Update:      added column qty and subtotal
-- =============================================
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	s.Id, 
			s.CreatedOn, 
			s.UpdatedOn, 
			s.Description, 
			SL.Description AS Item, 
			sl.qty,
			SL.Amount, 
			sl.subTotal,
			C.Name as Category, 
			L.Name as Location
	FROM Shoppings s
	LEFT OUTER JOIN ShoppingLists SL 
		ON S.ID = SL.ShoppingId
	LEFT OUTER JOIN Categories C 
		ON SL.CategoryId = C.Id
	LEFT OUTER JOIN Locations L
		ON SL.LocationId = L.id
	WHERE S.State = 1
	ORDER BY S.Id, SL.Id
	OFFSET (@PageNumber-1)*@RowsPage ROWS
	FETCH NEXT @RowsPage ROWS Only;

END");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}

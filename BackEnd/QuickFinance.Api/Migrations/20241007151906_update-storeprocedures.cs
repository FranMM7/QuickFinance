using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class updatestoreprocedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //Add Pagination to BudgetDetails 
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetBudgetDetails]
	@PageNumber as int = 1
AS
BEGIN
    -- =============================================
    -- Author:        Francis Edgardo Mejía Medina
    -- Create date:   24-09-25
    -- Description:   Retrieves budget details, including total and executed budgets for each month.
	-- Updated date:  2024-10-07
	-- Lastes changes: Added Pagination 
    -- =============================================
    SET NOCOUNT ON;
	Declare @RowsPage as int = 20

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
    GROUP BY Budgets.Id, Budgets.TotalBudget, Budgets.Month, Budgets.CreatedOn, Budgets.UpdatedOn
	ORDER BY Budgets.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END");

            //add pagination to category details
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetCategoryDetails]
	@PageNumber as int = 1
AS
BEGIN
	-- =============================================
	-- Author:		Francis E Mejia
	-- Create date: 24-09-25
	-- Description:	Will display basic information about the category
	-- Updated date:  2024-10-07
	-- Lastes changes: Added Pagination 
    -- =============================================
	SET NOCOUNT ON;

	Declare @RowsPage int = 20

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


            //add pagination to expenses
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetExpenseDetails]
	@BudgetId int,
	@PageNumber as int = 1
AS
BEGIN
-- =============================================
-- Author:        Francis Mejía 
-- Create date:   2024-09-30
-- Description:   Retrieves budget details, including total and executed budgets for each month.
-- Updated date:  2024-10-07
-- Lastes changes: Added pagination
-- =============================================
    SET NOCOUNT ON;

	Declare @RowsPage int = 20

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
        ON Expenses.PaymentMethodId = PaymentMethods.Id
	WHERE Budgets.Id = @BudgetId
	Order by Expenses.Id 
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END");

        }

        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}

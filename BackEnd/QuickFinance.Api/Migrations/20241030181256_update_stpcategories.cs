using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class update_stpcategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetCategoryDetails]
	@PageNumber as int = 1,
	@RowsPage int = 20
AS
BEGIN
	-- =============================================
	-- Author:		Francis E Mejia
	-- Create date: 24-09-25
	-- Description:	Will display basic information about the category
	-- Updated date:  2024-10-30
	-- Lastes changes: ModifiedOn, added to additonal columns for calculated the expenses increase base on categories, InUse column will block the delete button  
    -- =============================================
	SET NOCOUNT ON;

	--Declare @RowsPage int = 20

    SELECT 
        c.Id,
        c.Name,
		c.BudgetLimit,
        CASE WHEN COUNT(e.Id) > 0 THEN 1 
			 WHEN COUNT(SL.Id) > 0 THEN 1  
		ELSE 0 END AS InUse,
        COALESCE(SUM(E.Amount), 0) AS BudgetsTotalExpended,
		CAST(COALESCE(SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END), 0) AS decimal(18, 2)) AS BudgetsTotalExpendedExecuted,
		COALESCE(SUM(SL.subtotal), 0) AS ShoppingTotalExpended,
        ModifiedOn = CAST( CASE WHEN C.UpdatedOn IS NULL THEN C.CreatedOn ELSE C.UpdatedOn END AS DATE)
    FROM 
        Categories c
    LEFT JOIN 
        Expenses e ON e.CategoryId = c.Id
	LEFT JOIN 
		ShoppingLists SL ON SL.CategoryId = C.Id
	WHERE C.State = 1
    GROUP BY 
        c.Id, c.CreatedOn, c.UpdatedOn, c.Name, c.BudgetLimit
	ORDER BY c.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetCategoryDetails]
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

END
GO
");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class stp_PriceIncreaseCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
CREATE PROCEDURE stp_PriceIncreaseCategory
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- =============================================
	-- Author:		Francis M
	-- Create date: 2024-10-30
	-- Description:	Report to track the price increase by category
	-- =============================================

	
		DECLARE @cols NVARCHAR(MAX);
		DECLARE @query NVARCHAR(MAX);

		-- Step 1: Generate the column names dynamically using STUFF
		SET @cols = STUFF((SELECT DISTINCT ',' + QUOTENAME(CONCAT(YEAR(S.CreatedOn), '-', DATENAME(month, S.CreatedOn))) 
						   FROM Shoppings AS S
						   LEFT JOIN ShoppingLists AS SL ON S.Id = SL.ShoppingId
						   GROUP BY CONCAT(YEAR(S.CreatedOn), '-', DATENAME(month, S.CreatedOn))
						   FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '');

		-- Step 2: Construct the dynamic SQL for the pivot table
		SET @query = N'SELECT Category, ' + @cols + '
					   FROM 
					   (
						   SELECT  
								CONCAT(YEAR(S.CreatedOn), ''-'', DATENAME(month, S.CreatedOn)) AS YearMonth, 
								C.Name AS Category, 
								SUM(SL.Subtotal) AS Total
						   FROM 
								Shoppings AS S
						   LEFT JOIN 
								ShoppingLists AS SL ON S.Id = SL.ShoppingId
						   LEFT OUTER JOIN 
								Categories AS C ON SL.CategoryId = C.Id
						   GROUP BY 
							   CONCAT(YEAR(S.CreatedOn), ''-'', DATENAME(month, S.CreatedOn)), 
							   C.Name
					   ) AS SourceTable
					   PIVOT 
					   (
						   SUM(Total)
						   FOR YearMonth IN (' + @cols + ')
					   ) AS PivotTable
					   ORDER BY 
						   Category;';

		-- Step 3: Execute the dynamic SQL
		EXEC sp_executesql @query;
END
GO

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP PROCEDURE stp_PriceIncreaseCategory");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class report_stp_priceproductincrease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
CREATE PROCEDURE stp_PriceProductIncrease
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- =============================================
	-- Author:		Francis M
	-- Create date: 2024-10-30
	-- Description: Generates a pivot table of the price increase by month, base on product name 
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
	SET @query = N'SELECT Product, ' + @cols + '
				   FROM 
				   (
					   SELECT  
						   CONCAT(YEAR(S.CreatedOn), ''-'', DATENAME(month, S.CreatedOn)) AS YearMonth, 
						   SL.ItemName AS Product,
						   SUM(SL.Subtotal) AS Total
					   FROM 
						   Shoppings AS S
					   LEFT JOIN 
						   ShoppingLists AS SL ON S.Id = SL.ShoppingId
					   GROUP BY 
						   CONCAT(YEAR(S.CreatedOn), ''-'', DATENAME(month, S.CreatedOn)), 
						   SL.ItemName
				   ) AS SourceTable
				   PIVOT 
				   (
					   SUM(Total)
					   FOR YearMonth IN (' + @cols + ')
				   ) AS PivotTable
				   ORDER BY 
					   Product;';

	-- Step 3: Execute the dynamic SQL
	EXEC sp_executesql @query;
END
GO
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
DROP PROCEDURE [dbo].[stp_PriceProductIncrease]
GO
");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGetCategoryDetailsStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- =============================================
                -- Author:		Francis E Mejia
                -- Create date: 24-09-25
                -- Description:	Will display basic information about the category
                -- =============================================
                CREATE PROCEDURE [dbo].[GetCategoryDetails]
	                -- Add the parameters for the stored procedure here

                AS
                BEGIN
	                -- SET NOCOUNT ON added to prevent extra result sets from
	                -- interfering with SELECT statements.
	                SET NOCOUNT ON;

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
                        c.Id, c.CreatedOn, c.UpdatedOn, c.Name, c.BudgetLimit;


                END
                GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetCategoryDetails");
        }
    }
}

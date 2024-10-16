using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class createstp_shopping_financeevaluations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

			//add description properties to the expensetype column 
			migrationBuilder.Sql(@"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 = Important
2 = Ghost Expense
3 = Ant Expense
4 = Vampire Expense' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FinanceDetails', @level2type=N'COLUMN',@level2name=N'ExpenseType'
GO");


			//add stp_getFinanceEvaluationList
			migrationBuilder.Sql(@"CREATE PROCEDURE stp_getfinanceEvaluations
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
			CASE WHEN FD.ExpenseType = 1 THEN 'IMPORTANT' 
				 WHEN FD.ExpenseType = 2 THEN 'Ghost Expense' 
				 WHEN FD.ExpenseType = 3 THEN 'Ant Expense' 
				 WHEN FD.ExpenseType = 4 THEN 'Vampire Expense' 
			ELSE 'OTHER/ND' END ExpenseType,
			Amount
	FROM FinanceEvaluations FE
	LEFT OUTER JOIN FinanceDetails FD 
		ON FE.Id = FD.FinanceId
	LEFT OUTER JOIN Categories C
		ON FD.CategoryId = C.Id
	ORDER BY FE.Id, FD.Id DESC
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END
GO
");

            //add Stp_getShoppinglist
            migrationBuilder.Sql(@"CREATE PROCEDURE Stp_getShoppinglist
	-- Add the parameters for the stored procedure here
	@PageNumber int = 1,
	@RowsPage int = 20
AS
BEGIN
-- =============================================
-- Author:		FRANCIS MEJIA
-- Create date: 2024-10-14
-- Description:	Shopping Pagination 
-- =============================================
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	s.Id, 
			s.CreatedOn, 
			s.UpdatedOn, 
			s.Description, 
			SL.Description AS Item, 
			SL.Amount, 
			C.Name as Category, 
			L.Name as Location
	FROM Shoppings s
	LEFT OUTER JOIN ShoppingLists SL 
		ON S.ID = SL.ShoppingId
	LEFT OUTER JOIN Categories C 
		ON SL.CategoryId = C.Id
	LEFT OUTER JOIN Locations L
		ON SL.LocationId = L.id
	ORDER BY S.Id, SL.Id
	OFFSET (@PageNumber-1)*@RowsPage ROWS
	FETCH NEXT @RowsPage ROWS Only;

END
GO");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //drop stp_getShoppingList
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[stp_getfinanceEvaluations]");

            //drop stp_getShoppingList
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[Stp_getShoppinglist]");
        }
    }
}

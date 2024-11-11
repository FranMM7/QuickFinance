using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class update_stp_getfinanceEvaluations_111124 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[stp_getfinanceEvaluations]
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
	-- Update date: 2024-11-11
	-- Change into retreving only the data from Finance Evaluation table, and added the total income and expenses and modifiedON column
	-- =============================================
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	FE.Id,
			FE.Title,
			modifiedON= CASE WHEN FE.UpdatedOn IS NULL THEN FE.CreatedOn ELSE FE.UpdatedOn END,
			TotalIncomes,
			TotalExpenses
	FROM FinanceEvaluations FE
	WHERE FE.State=1
	ORDER BY FE.Id DESC
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[stp_getfinanceEvaluations]
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
END");
        }
    }
}

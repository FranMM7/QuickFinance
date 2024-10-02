using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class update_GetExpenseDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetExpenseDetails]
	@BudgetId int
AS
BEGIN
-- =============================================
-- Author:        Francis Mejía 
-- Create date:   2024-09-30
-- Description:   Retrieves budget details, including total and executed budgets for each month.
-- Updated date:  2024-10-02
-- Lastes changes: Added the BudgetId parameter
-- =============================================
    SET NOCOUNT ON;

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
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}

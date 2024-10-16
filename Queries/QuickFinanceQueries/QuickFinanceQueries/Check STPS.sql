USE [QuickFinanceDB]
GO

DECLARE @RC int
DECLARE @PageNumber int =1 
DECLARE @RowsPage int = 10
DECLARE @BudgetID int =1 

-- TODO: Set parameter values here.
EXECUTE @RC = [dbo].[GetBudgetDetails] 
   @PageNumber
  ,@RowsPage


-- TODO: Set parameter values here.
EXECUTE @RC = [dbo].[GetCategoryDetails] 
   @PageNumber
  ,@RowsPage


EXECUTE @RC = [dbo].[GetExpenseDetails] 
   @BudgetId
  ,@PageNumber
  ,@RowsPage

-- TODO: Set parameter values here.
EXECUTE @RC = [dbo].[sp_GetBudgetOverviewJSON] 


-- TODO: Set parameter values here.

EXECUTE @RC = [dbo].[stp_getfinanceEvaluations] 
   @PageNumber
  ,@RowsPage


-- TODO: Set parameter values here.

EXECUTE @RC = [dbo].[Stp_getShoppinglist] 
   @PageNumber
  ,@RowsPage

GO



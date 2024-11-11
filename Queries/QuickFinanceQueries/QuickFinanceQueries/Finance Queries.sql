USE QuickFinanceDB

SELECT        Id, CreatedOn, UpdatedOn, Title, State
FROM            FinanceEvaluations

SELECT        Id, FinanceId, Description, ExpenseCategory, Amount, CategoryId
FROM            FinanceDetails
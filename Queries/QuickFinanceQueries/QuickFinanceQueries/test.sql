declare @JsonResult nvarchar(max)

set @JsonResult= (SELECT 
    (SELECT TOP (5) 
        B.Month, 
        B.TotalBudget,
        SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Expenses,
        B.TotalBudget - SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Saving
    FROM Budgets AS B 
    LEFT OUTER JOIN Expenses AS E 
        ON E.BudgetId = B.Id
    GROUP BY B.Id, B.Month, B.CreatedOn, B.TotalBudget
    ORDER BY B.CreatedOn DESC
    FOR JSON PATH) AS BudgetTop5,
    
    (SELECT TOP 1 
        B.Month, 
        B.TotalBudget,
        SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Expenses,
        B.TotalBudget - SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Saving
    FROM Budgets AS B 
    LEFT OUTER JOIN Expenses AS E 
        ON E.BudgetId = B.Id
    WHERE YEAR(B.CreatedOn) = YEAR(GETDATE()) 
    GROUP BY B.Id, B.Month, B.CreatedOn, B.TotalBudget
    HAVING SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) >= 0
    ORDER BY B.CreatedOn DESC
    FOR JSON PATH) AS MonthWithHighestExpenses
FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)


select @JsonResult
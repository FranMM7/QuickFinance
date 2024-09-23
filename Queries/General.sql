-- 1. General View of the Records
-- The query is mostly fine, but can be improved for readability and efficiency by ensuring best practices like avoiding square brackets (unless absolutely necessary) and using meaningful aliases.

SELECT 
    B.Month, 
    B.TotalBudget, 
    E.Description, 
    C.Name AS Category, 
    E.Amount, 
    E.DueDate, 
    PM.Name AS PaymentMethod, 
    E.Executed
FROM 
    Budgets B
INNER JOIN 
    Expenses E ON B.Id = E.BudgetId
INNER JOIN 
    Categories C ON C.Id = E.CategoryId
INNER JOIN 
    PaymentMethods PM ON PM.Id = E.PaymentMethodId;


-- Table aliases (B, E, C, PM) make the query cleaner.
-- Used more standard SQL conventions for column names instead of square brackets unless it's mandatory due to DBMS constraints.
-- 2. View of the Remaining Balance of the Executed Budget
-- This query can be optimized by using a SUM with a CASE to directly compute the balance without needing multiple aggregate functions.


SELECT 
    B.Month, 
    B.TotalBudget, 
    B.TotalBudget - SUM(CASE WHEN E.Executed = 1 THEN E.Amount ELSE 0 END) AS Balance
FROM 
    Budgets B
INNER JOIN 
    Expenses E ON B.Id = E.BudgetId
GROUP BY 
    B.Month, B.TotalBudget;

-- Instead of directly subtracting SUM(Amount), we only sum amounts for executed expenses using CASE WHEN E.Executed = 1 THEN E.Amount ELSE 0 END. This avoids grouping logic issues.
-- Maintains clarity and improves performance since we only compute the relevant amounts once.
-- 3. View Total of the Executed Records vs the Unexecuted Records and Remaining Balance
-- In the current query, you're using scalar subqueries for Executed, PendingExecuted, and Balance. This can be inefficient. Instead, we can achieve the same using a GROUP BY with conditional aggregation (SUM(CASE ...)) to avoid multiple subqueries.

SELECT 
    B.Month, 
    B.TotalBudget, 
    SUM(CASE WHEN E.Executed = 1 THEN E.Amount ELSE 0 END) AS Executed, 
    SUM(CASE WHEN E.Executed = 0 THEN E.Amount ELSE 0 END) AS PendingExecuted, 
    B.TotalBudget - SUM(CASE WHEN E.Executed = 1 THEN E.Amount ELSE 0 END) AS Balance
FROM 
    Budgets B
LEFT JOIN 
    Expenses E ON B.Id = E.BudgetId
GROUP BY 
    B.Month, B.TotalBudget;

-- Avoided subqueries by using SUM(CASE ...) for executed and pending amounts, improving performance.
-- Used LEFT JOIN to ensure all budgets are included, even those without any expenses yet.
-- This approach ensures that executed, pending, and balance calculations are done in a single pass, which is more efficient.
-- Summary of Key Changes:
-- Aliasing for clarity – Using meaningful table aliases improves readability and reduces code duplication.
-- Conditional Aggregation – By using SUM(CASE WHEN ...), we can avoid subqueries and group calculations.
-- LEFT JOIN – In cases where not all budgets may have expenses, using LEFT JOIN ensures all budgets are considered in the result.
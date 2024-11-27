use QuickFinanceDB
---price increase by product
SELECT 
    sl.ItemName, 
    SUM(sl.Subtotal / sl.Quantity) AS TotalByItem, 
    MIN(unit_price) AS LowestPrice, 
    MAX(unit_price) AS HighestPrice, 
    (MAX(unit_price) - MIN(unit_price)) / NULLIF(MIN(unit_price), 0) * 100 AS IncreasePercentage, 
    s.UserId
FROM 
    ShoppingLists sl
LEFT JOIN 
    Shoppings s ON sl.ShoppingId = s.Id
CROSS APPLY 
    (SELECT sl.Subtotal / sl.Quantity AS unit_price) AS price_calc
GROUP BY 
    sl.ItemName, 
    s.UserId;

SELECT 
    sl.ItemName, 
    SUM(sl.Amount) AS TotalByItem, 
    MIN(Amount) AS LowestPrice, 
    MAX(Amount) AS HighestPrice, 
    (MAX(Amount) - MIN(Amount)) / NULLIF(MIN(Amount), 0) * 100 AS IncreasePercentage, 
    s.UserId
FROM 
    ShoppingLists sl
LEFT JOIN 
    Shoppings s ON sl.ShoppingId = s.Id
GROUP BY 
    sl.ItemName, 
    s.UserId;


--price increase by brand and procut 
--USE QuickFinanceDB;
SELECT 
    CASE 
        WHEN SL.Brand IS NULL OR SL.Brand = '' THEN 'N/D' 
        ELSE SL.Brand 
    END AS Brand,
    sl.ItemName, 
    SUM(sl.Amount) AS TotalByItem, 
    MIN(Amount) AS LowestPrice, 
    MAX(Amount) AS HighestPrice, 
    (MAX(Amount) - MIN(Amount)) / NULLIF(MIN(Amount), 0) * 100 AS IncreasePercentage, 
    s.UserId
FROM 
    ShoppingLists sl
LEFT JOIN 
    Shoppings s ON sl.ShoppingId = s.Id
GROUP BY 
    CASE 
        WHEN SL.Brand IS NULL OR SL.Brand = '' THEN 'N/D' 
        ELSE SL.Brand 
    END, 
    sl.ItemName, 
    s.UserId;


--product increase base on category 
--USE QuickFinanceDB;

SELECT 
    C.Name AS Category,
    sl.ItemName, 
      SUM(sl.Amount) AS TotalByItem, 
    MIN(Amount) AS LowestPrice, 
    MAX(Amount) AS HighestPrice, 
    (MAX(Amount) - MIN(Amount)) / NULLIF(MIN(Amount), 0) * 100 AS IncreasePercentage, 
    s.UserId
FROM 
    ShoppingLists sl
LEFT JOIN 
    Shoppings s ON sl.ShoppingId = s.Id
LEFT JOIN 
	Categories C ON SL.CategoryId = C.Id
GROUP BY 
    C.Name, 
    sl.ItemName, 
    s.UserId;



---privot table price increas base on month year USE QuickFinanceDB;

USE QuickFinanceDB;

SELECT	CONCAT(YEAR(S.CreatedOn), '-', DATENAME(month, S.CreatedOn))  AS YearMonth, 
		SL.ItemName AS Product, 
		SUM(SL.Subtotal) AS Total
FROM Shoppings AS S 
LEFT OUTER JOIN ShoppingLists AS SL ON S.Id = SL.ShoppingId
GROUP BY  CONCAT(YEAR(S.CreatedOn), '-', DATENAME(month, S.CreatedOn)) , SL.ItemName;

USE QuickFinanceDB;

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



--========== report category price increase timeline 

SELECT	CONCAT(YEAR(S.CreatedOn), '-', DATENAME(month, S.CreatedOn))  AS YearMonth, 
		C.Name AS Category, 
		SUM(SL.Subtotal) AS Total
FROM Shoppings AS S 
LEFT OUTER JOIN ShoppingLists AS SL ON S.Id = SL.ShoppingId
LEFT OUTER JOIN Categories AS C ON SL.CategoryId = C.Id
GROUP BY  CONCAT(YEAR(S.CreatedOn), '-', DATENAME(month, S.CreatedOn)) , C.Name

USE QuickFinanceDB 
GO 

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
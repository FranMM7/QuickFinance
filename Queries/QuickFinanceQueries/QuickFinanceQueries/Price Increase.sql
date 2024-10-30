use QuickFinanceDB
---price increase by product
SELECT 
    ItemName, 
    SUM(subtotal) AS TotalByItem,
    MIN(subtotal) AS LowestPrice,
    MAX(subtotal) AS HighestPrice, 
    (MAX(subtotal) - MIN(subtotal)) / NULLIF(MIN(subtotal), 0) * 100 AS IncreasePercentage 
FROM 
    ShoppingLists
GROUP BY 
    ItemName;

--price increase by brand and procut 
--USE QuickFinanceDB;

SELECT 
    Brand,
    ItemName, 
    SUM(subtotal) AS TotalByItem,
    MIN(subtotal) AS LowestPrice,
    MAX(subtotal) AS HighestPrice, 
    (MAX(subtotal) - MIN(subtotal)) / NULLIF(MIN(subtotal), 0) * 100 AS IncreasePercentage 
FROM 
    ShoppingLists
GROUP BY 
    Brand, ItemName;


--product increase base on category 
--USE QuickFinanceDB;

SELECT 
    c.Name AS Category, 
    ItemName, 
    SUM(sl.subtotal) AS TotalByItem,
    MIN(sl.subtotal) AS LowestPrice,
    MAX(sl.subtotal) AS HighestPrice, 
    (MAX(sl.subtotal) - MIN(sl.subtotal)) / NULLIF(MIN(sl.subtotal), 0) * 100 AS IncreasePercentage 
FROM 
    ShoppingLists sl 
LEFT JOIN 
    Categories c ON sl.CategoryId = c.id 
GROUP BY 
    c.Name, ItemName;



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

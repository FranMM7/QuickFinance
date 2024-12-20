USE [QuickFinanceDB]
GO
/****** Object:  View [dbo].[vw_PriceIncreaseByBrandAndProduct]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_PriceIncreaseByBrandAndProduct]
AS
SELECT        dbo.ShoppingLists.Brand, dbo.ShoppingLists.ItemName, SUM(dbo.ShoppingLists.Subtotal) AS TotalByItem, MIN(dbo.ShoppingLists.Subtotal) AS LowestPrice, MAX(dbo.ShoppingLists.Subtotal) AS HighestPrice, 
                         (MAX(dbo.ShoppingLists.Subtotal) - MIN(dbo.ShoppingLists.Subtotal)) / NULLIF (MIN(dbo.ShoppingLists.Subtotal), 0) * 100 AS IncreasePercentage, dbo.Shoppings.UserId
FROM            dbo.ShoppingLists INNER JOIN
                         dbo.Shoppings ON dbo.ShoppingLists.ShoppingId = dbo.Shoppings.Id
GROUP BY dbo.ShoppingLists.Brand, dbo.ShoppingLists.ItemName, dbo.Shoppings.UserId
GO
/****** Object:  View [dbo].[vw_PriceIncreaseByCategoryAndProduct]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_PriceIncreaseByCategoryAndProduct]
AS
SELECT        c.Name AS Category, sl.ItemName, SUM(sl.Subtotal) AS TotalByItem, MIN(sl.Subtotal) AS LowestPrice, MAX(sl.Subtotal) AS HighestPrice, (MAX(sl.Subtotal) - MIN(sl.Subtotal)) / NULLIF (MIN(sl.Subtotal), 0) 
                         * 100 AS IncreasePercentage, s.UserId
FROM            dbo.ShoppingLists AS sl LEFT OUTER JOIN
                         dbo.Shoppings AS s ON sl.ShoppingId = s.Id LEFT OUTER JOIN
                         dbo.Categories AS c ON sl.CategoryId = c.Id
GROUP BY c.Name, sl.ItemName, s.UserId
GO
/****** Object:  View [dbo].[vw_PriceIncreaseByProduct]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_PriceIncreaseByProduct]
AS
SELECT        dbo.ShoppingLists.ItemName, SUM(dbo.ShoppingLists.Subtotal) AS TotalByItem, MIN(dbo.ShoppingLists.Subtotal) AS LowestPrice, MAX(dbo.ShoppingLists.Subtotal) AS HighestPrice, (MAX(dbo.ShoppingLists.Subtotal) 
                         - MIN(dbo.ShoppingLists.Subtotal)) / NULLIF (MIN(dbo.ShoppingLists.Subtotal), 0) * 100 AS IncreasePercentage, dbo.Shoppings.UserId
FROM            dbo.ShoppingLists LEFT OUTER JOIN
                         dbo.Shoppings ON dbo.ShoppingLists.ShoppingId = dbo.Shoppings.Id
GROUP BY dbo.ShoppingLists.ItemName, dbo.Shoppings.UserId
GO
/****** Object:  StoredProcedure [dbo].[stp_AddUserCategoriesAndLocations]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[stp_AddUserCategoriesAndLocations]
    @userID NVARCHAR(450),
    @lang NVARCHAR(50) = 'ENG' -- Default to English
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
	-- =============================================
	-- Author:		Francis M
	-- Create date: 2024-11-14
	-- Description: When a new user is register 
	-- =============================================
    BEGIN TRY


        -- Insert Category and Locations records with language-specific names
        IF @lang = 'ENG'
        BEGIN
			INSERT INTO Locations (Name, UserId, State) VALUES ('Local-Market', @userID, 1);

            INSERT INTO Categories (Name, BudgetLimit, TypeBudget, TypeShoppingList, TypeFinanceAnalizis, UserId, State)
            VALUES 
                ('Food', 0, 1, 0, 1, @userID,1),
                ('Transport', 0, 1, 0, 0, @userID,1),
                ('Entertainment', 0, 1, 1, 1, @userID,1),
                ('Dairy', 0, 1, 1, 0, @userID,1),
                ('Meats', 0, 1, 1, 0, @userID,1),
                ('Cleaning', 0, 1, 1, 1, @userID,1),
                ('Utilities',0, 1, 1, 1, @userID,1),
                ('Health', 0, 1, 1, 1, @userID,1),
				('Home', 0, 1, 1, 1, @userID,1);
        END
        ELSE IF @lang = 'SPA'
        BEGIN
			INSERT INTO Locations (Name, UserId, State) VALUES ('Mercado-Local', @userID,1);

            INSERT INTO Categories (Name, BudgetLimit, TypeBudget, TypeShoppingList, TypeFinanceAnalizis, UserId,State)
            VALUES 
                ('Alimentos', 0, 1, 0, 1, @userID,1),
                ('Transporte', 0, 1, 0, 0, @userID,1),
                ('Entretenimiento',0, 1, 1, 1, @userID,1),
                ('Lácteos', 0, 1, 1, 0, @userID,1),
                ('Carnes', 0, 1, 1, 0, @userID,1),
                ('Limpieza', 0, 1, 1, 1, @userID,1),
                ('Servicios Públicos', 0, 1, 1, 1, @userID,1),
                ('Salud', 0, 1, 1, 1, @userID,1),
				('Vivienda', 0, 1, 1, 1, @userID,1);
        END
		ELSE
		BEGIN
			INSERT INTO Locations (Name, UserId, State) VALUES ('Local-Market', @userID, 1);

            INSERT INTO Categories (Name, BudgetLimit, TypeBudget, TypeShoppingList, TypeFinanceAnalizis, UserId, State)
            VALUES 
                ('Food', 300.00, 1, 0, 1, @userID,1),
                ('Transport', 20.20, 1, 0, 0, @userID,1),
                ('Entertainment', 100.00, 1, 1, 1, @userID,1),
                ('Dairy', 100.00, 1, 1, 0, @userID,1),
                ('Meats', 100.00, 1, 1, 0, @userID,1),
                ('Cleaning', 100.00, 1, 1, 1, @userID,1),
                ('Utilities', 200.00, 1, 1, 1, @userID,1),
                ('Health', 150.00, 1, 1, 1, @userID,1),
				('Home', 0, 1, 1, 1, @userID,1);
		END
			

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; -- Rethrow error to handle it externally if needed
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[stp_CloneShoppingList]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[stp_CloneShoppingList]
	@Id INT
AS
BEGIN
	-- =============================================
	-- Author:       FRANCIS M
	-- Create date: 2024-11-6
	-- Description:  Clone a Shopping List 
	-- =============================================
	SET NOCOUNT ON;

	-- Declare variables
	DECLARE @Description NVARCHAR(50),
			@userID nvarchar(450),
			@CurrentDateString NVARCHAR(50),
			@newId INT;

	-- Start transaction and try block
	BEGIN TRANSACTION;

	BEGIN TRY
		-- Set the current date in string format
		SET @CurrentDateString = CONCAT(YEAR(GETDATE()), '-', MONTH(GETDATE()), '-', DAY(GETDATE()));

		-- Retrieve the description of the shopping list to clone
		SELECT	@Description = Description,
				@userID=UserId
		FROM Shoppings
		WHERE Id = @Id;

		-- Check if the temporary table exists and drop it if it does
		IF OBJECT_ID('tempdb..#tmpList') IS NOT NULL
			DROP TABLE #tmpList;

		-- Create a temporary table and copy the shopping list items
		SELECT CategoryId, LocationId, ItemName, Brand, Quantity, Amount = 0
		INTO #tmpList
		FROM ShoppingLists
		WHERE ShoppingId = @Id;

		-- Insert a new entry in the Shoppings table with the cloned description
		INSERT INTO Shoppings ([Description], UserId, State) 
		VALUES (CONCAT(@Description, ' COPY ', @CurrentDateString), @userID,1);
		
		-- Get the newly generated ID for the cloned shopping list
		SELECT @newId = SCOPE_IDENTITY();

		-- Insert the cloned items into the ShoppingLists table
		INSERT INTO ShoppingLists (ShoppingId, CategoryId, LocationId, ItemName, Brand, Quantity, Amount)
		SELECT @newId, CategoryId, LocationId, ItemName, Brand, Quantity, Amount
		FROM #tmpList;

		-- Drop the temporary table
		DROP TABLE #tmpList;

		-- Commit transaction if all operations are successful
		COMMIT TRANSACTION;

		-- Return the newly cloned shopping list record
		SELECT * FROM Shoppings WHERE Id = @newId;

	END TRY

	BEGIN CATCH
		-- Rollback transaction in case of an error
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;

		-- Capture and return detailed error information
		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
		SELECT @ErrorMessage = ERROR_MESSAGE(), 
			   @ErrorSeverity = ERROR_SEVERITY(), 
			   @ErrorState = ERROR_STATE();

		-- Rethrow the error with detailed information
		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[stp_GetBudgetDetails]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[stp_GetBudgetDetails]
	@userId nvarchar(450),
	@PageNumber as int = 1,
	@RowsPage as int = 20
AS
BEGIN
    -- =============================================
    -- Author:        Francis Edgardo Mejía Medina
    -- Create date:   24-09-25
    -- Description:   Retrieves budget details, including total and executed budgets for each month.
	-- Updated date:  2024-11-18
	-- Lastes changes: rename stp, add userid as parameter 
    -- =============================================
    SET NOCOUNT ON;

    SELECT 
        Budgets.Id, 
        Budgets.TotalAllocatedBudget, 
        SUM(CASE WHEN Expenses.IsExecuted = 1 THEN Expenses.Amount ELSE 0 END) AS ExecutedBudget,
        Budgets.Title,
        CASE 
            WHEN Budgets.UpdatedOn IS NULL THEN Budgets.CreatedOn 
            ELSE Budgets.UpdatedOn 
        END AS ModifiedOn
    FROM Budgets
    LEFT OUTER JOIN Expenses ON Expenses.BudgetId = Budgets.Id
	WHERE UserId=@userId
	AND Budgets.State = 1
    GROUP BY Budgets.Id, Budgets.TotalAllocatedBudget, Budgets.Title, Budgets.CreatedOn, Budgets.UpdatedOn
	ORDER BY Budgets.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END
GO
/****** Object:  StoredProcedure [dbo].[stp_GetBudgetOverviewJSON]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stp_GetBudgetOverviewJSON]
	@userId nvarchar(450)
AS 
BEGIN 
    -- =============================================
    -- Author:        FRANCIS MEJIA
    -- Create date:   2024-10-7
    -- Description:   Retrives a summary of the top 5 budgets and the month with the hightest expenses 
    -- Modified On:   2024-11-18
    -- Update:        Update to add UserId as parameter 
    -- =============================================
    Declare @JsonResult nvarchar(max)

	SET @JsonResult = (
		SELECT 
			-- Fetch top 5 budgets
			(
				SELECT TOP (5)
					B.Id AS BudgetId,
					B.Title, 
					B.TotalAllocatedBudget AS Total,
					COALESCE(SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END), 0) AS Expenses,
					B.TotalAllocatedBudget - COALESCE(SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END), 0) AS Saving
				FROM Budgets AS B
				LEFT OUTER JOIN Expenses AS E 
					ON E.BudgetId = B.Id
				WHERE B.State = 1
				  AND B.UserId = @userId
				GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalAllocatedBudget
				ORDER BY B.CreatedOn DESC
				FOR JSON PATH
			) AS BudgetTop5,
        
			-- Fetch the record with the highest expenses for the current year
			(
				SELECT TOP (1)
					B.Id AS BudgetId,
					B.Title, 
					B.TotalAllocatedBudget,
					COALESCE(SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END), 0) AS Expenses,
					B.TotalAllocatedBudget - COALESCE(SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END), 0) AS Saving
				FROM Budgets AS B
				LEFT OUTER JOIN Expenses AS E 
					ON E.BudgetId = B.Id
				WHERE YEAR(B.CreatedOn) = YEAR(GETDATE())
				  AND B.State = 1
				  AND B.UserId = @userId
				GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalAllocatedBudget
				HAVING COALESCE(SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END), 0) >= 0
				ORDER BY Expenses DESC
				FOR JSON PATH
			) AS RecordWithHighestExpenses
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
	);



    SELECT @JsonResult               
END


GO
/****** Object:  StoredProcedure [dbo].[stp_GetCategoryDetails]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stp_GetCategoryDetails]
	@userId nvarchar(450),
	@PageNumber as int = 1,
	@RowsPage int = 20
AS
BEGIN
	-- =============================================
	-- Author:		Francis E Mejia
	-- Create date: 24-09-25
	-- Description:	Will display basic information about the category
	-- Updated date:  2024-10-30
	-- Lastes changes: ModifiedOn, added to additonal columns for calculated the expenses increase base on categories, InUse column will block the delete button  
    -- =============================================
	SET NOCOUNT ON;

	--Declare @RowsPage int = 20

    SELECT 
        c.Id,
        c.Name,
		c.BudgetLimit,
        CASE WHEN COUNT(e.Id) > 0 THEN 1 
			 WHEN COUNT(SL.Id) > 0 THEN 1  
		ELSE 0 END AS InUse,
        COALESCE(SUM(E.Amount), 0) AS BudgetsTotalExpended,
		CAST(COALESCE(SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END), 0) AS decimal(18, 2)) AS BudgetsTotalExpendedExecuted,
		COALESCE(SUM(SL.subtotal), 0) AS ShoppingTotalExpended,
        ModifiedOn = CAST( CASE WHEN C.UpdatedOn IS NULL THEN C.CreatedOn ELSE C.UpdatedOn END AS DATE)
    FROM 
        Categories c
    LEFT JOIN 
        Expenses e ON e.CategoryId = c.Id
	LEFT JOIN 
		ShoppingLists SL ON SL.CategoryId = C.Id
	WHERE userId=@userId
	AND C.State = 1
    GROUP BY 
        c.Id, c.CreatedOn, c.UpdatedOn, c.Name, c.BudgetLimit
	ORDER BY c.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END

GO
/****** Object:  StoredProcedure [dbo].[stp_GetExpenseDetails]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[stp_GetExpenseDetails]
	@userId nvarchar(450),
	@BudgetId int,
	@PageNumber as int = 1,
	@RowsPage int = 20
AS
BEGIN
-- =============================================
-- Author:        Francis Mejía 
-- Create date:   2024-09-30
-- Description:   Retrieves budget details, including total and executed budgets for each month.
-- Updated date:  2024-11-18
-- Lastes changes: rename stp, add userid as parameter 
-- =============================================
    SET NOCOUNT ON;

	--Declare @RowsPage int = 20

    SELECT 
        Expenses.Id, 
        Expenses.Description, 
        Expenses.Amount, 
        Expenses.ExpenseDueDate, 
        Expenses.CategoryId, 
        Categories.Name AS Category, 
        Expenses.BudgetId, 
        Budgets.Title, 
        Budgets.TotalAllocatedBudget, 
        Expenses.PaymentMethodId, 
        PaymentMethodName AS PaymentMethod,
        Expenses.IsExecuted, 
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
	WHERE Budgets.UserId=@userId
	AND Budgets.Id = @BudgetId
	Order by Expenses.Id 
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END


GO
/****** Object:  StoredProcedure [dbo].[stp_getfinanceEvaluations]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stp_getfinanceEvaluations]
	-- Add the parameters for the stored procedure here
	@userId nvarchar(450),
	@PageNumber int = 1,
	@RowsPage int = 20
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- =============================================
	-- Author:		Francis Mejia
	-- Create date: 2024-10-14
	-- Description:	Finance Evaluation pagination // 1 = Important 2 = Ghost Expense 3 = Ant Expense 4 = Vampire Expense
	-- Update date: 2024-11-18
	-- add user parameter
	-- =============================================
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	FE.Id,
			FE.Title,
			modifiedON= CASE WHEN FE.UpdatedOn IS NULL THEN FE.CreatedOn ELSE FE.UpdatedOn END,
			TotalIncomes,
			TotalExpenses
	FROM FinanceEvaluations FE
	WHERE UserId=@userId
	AND FE.State=1
	ORDER BY FE.Id DESC
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END

GO
/****** Object:  StoredProcedure [dbo].[Stp_getShoppinglist]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Stp_getShoppinglist]
	-- Add the parameters for the stored procedure here
	@PageNumber int = 1,
	@RowsPage int = 20
AS
BEGIN
-- =============================================
-- Author:		FRANCIS MEJIA
-- Create date: 2024-10-14
-- Description:	Shopping Pagination 
-- Update date: 2024-10-14
-- Update:      added column qty and subtotal
-- =============================================
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	s.Id, 
			s.CreatedOn, 
			s.UpdatedOn, 
			s.Description, 
			SL.ItemName, 
		    sl.Quantity,
			SL.Amount, 
			sl.subTotal,
			C.Name as Category, 
			L.Name as Location
	FROM Shoppings s
	LEFT OUTER JOIN ShoppingLists SL 
		ON S.ID = SL.ShoppingId
	LEFT OUTER JOIN Categories C 
		ON SL.CategoryId = C.Id
	LEFT OUTER JOIN Locations L
		ON SL.LocationId = L.id
	WHERE S.State = 1
	ORDER BY S.Id, SL.Id
	OFFSET (@PageNumber-1)*@RowsPage ROWS
	FETCH NEXT @RowsPage ROWS Only;

END
GO
/****** Object:  StoredProcedure [dbo].[stp_PriceIncreaseCategory]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[stp_PriceIncreaseCategory]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- =============================================
	-- Author:		Francis M
	-- Create date: 2024-10-30
	-- Description:	Report to track the price increase by category
	-- =============================================

	
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
END
GO
/****** Object:  StoredProcedure [dbo].[stp_PriceProductIncrease]    Script Date: 11/20/2024 11:59:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stp_PriceProductIncrease]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- =============================================
	-- Author:		Francis M
	-- Create date: 2024-10-30
	-- Description: Generates a pivot table of the price increase by month, base on product name 
-- =============================================
	

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
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ShoppingLists"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 288
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Shoppings"
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 236
               Right = 448
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_PriceIncreaseByBrandAndProduct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_PriceIncreaseByBrandAndProduct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[29] 4[32] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "sl"
            Begin Extent = 
               Top = 0
               Left = 285
               Bottom = 130
               Right = 471
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 1
               Left = 0
               Bottom = 131
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s"
            Begin Extent = 
               Top = 0
               Left = 625
               Bottom = 183
               Right = 811
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_PriceIncreaseByCategoryAndProduct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_PriceIncreaseByCategoryAndProduct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[33] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ShoppingLists"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Shoppings"
            Begin Extent = 
               Top = 0
               Left = 418
               Bottom = 130
               Right = 604
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_PriceIncreaseByProduct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_PriceIncreaseByProduct'
GO

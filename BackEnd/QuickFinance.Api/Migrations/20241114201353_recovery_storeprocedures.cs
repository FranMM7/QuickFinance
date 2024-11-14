using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class recovery_storeprocedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region storeprocedures_triggers
            //create store precedure  AddUserCategoriesAndLocations
            migrationBuilder.Sql(@"
CREATE PROCEDURE AddUserCategoriesAndLocations
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
			INSERT INTO Locations (Name, UserId) VALUES ('Local-Market', @userID);

            INSERT INTO Categories (Name, BudgetLimit, TypeBudget, TypeShoppingList, TypeFinanceAnalizis, UserId)
            VALUES 
                ('Food', 300.00, 1, 0, 1, @userID),
                ('Transport', 20.20, 1, 0, 0, @userID),
                ('Entertainment', 100.00, 1, 1, 1, @userID),
                ('Dairy', 100.00, 1, 1, 0, @userID),
                ('Meats', 100.00, 1, 1, 0, @userID),
                ('Cleaning', 100.00, 1, 1, 1, @userID),
                ('Utilities', 200.00, 1, 1, 1, @userID),
                ('Health', 150.00, 1, 1, 1, @userID);
        END
        ELSE IF @lang = 'SPA'
        BEGIN
			INSERT INTO Locations (Name, UserId) VALUES ('Mercado-Local', @userID);

            INSERT INTO Categories (Name, BudgetLimit, TypeBudget, TypeShoppingList, TypeFinanceAnalizis, UserId)
            VALUES 
                ('Alimentos', 300.00, 1, 0, 1, @userID),
                ('Transporte', 20.20, 1, 0, 0, @userID),
                ('Entretenimiento', 100.00, 1, 1, 1, @userID),
                ('Lácteos', 100.00, 1, 1, 0, @userID),
                ('Carnes', 100.00, 1, 1, 0, @userID),
                ('Limpieza', 100.00, 1, 1, 1, @userID),
                ('Servicios Públicos', 200.00, 1, 1, 1, @userID),
                ('Salud', 150.00, 1, 1, 1, @userID);
        END
		ELSE
		BEGIN
			INSERT INTO Locations (Name, UserId) VALUES ('Local-Market', @userID);

            INSERT INTO Categories (Name, BudgetLimit, TypeBudget, TypeShoppingList, TypeFinanceAnalizis, UserId)
            VALUES 
                ('Food', 300.00, 1, 0, 1, @userID),
                ('Transport', 20.20, 1, 0, 0, @userID),
                ('Entertainment', 100.00, 1, 1, 1, @userID),
                ('Dairy', 100.00, 1, 1, 0, @userID),
                ('Meats', 100.00, 1, 1, 0, @userID),
                ('Cleaning', 100.00, 1, 1, 1, @userID),
                ('Utilities', 200.00, 1, 1, 1, @userID),
                ('Health', 150.00, 1, 1, 1, @userID);
		END
			

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; -- Rethrow error to handle it externally if needed
    END CATCH
END;
GO

");

            //add trigger tg_createUsersSettings
            migrationBuilder.Sql(@"

CREATE TRIGGER tg_createUsersSettings
   ON AspNetUsers
   AFTER INSERT
AS 
BEGIN
    -- =============================================
    -- Author:       Francis M
    -- Create date:  2024-11-14
    -- Description:  Trigger to initialize user settings upon registration.
    -- =============================================
    SET NOCOUNT ON;

    DECLARE @userID NVARCHAR(450),
            @JsonValue NVARCHAR(MAX),
            @Lang NVARCHAR(50);

    -- Get the new user's ID from the inserted row
    SELECT @userID = Id
    FROM inserted;

    -- Retrieve JSON settings for the 'Cultural information' setting for this user
    SELECT @JsonValue = JsonValue
    FROM [dbo].[Settings] 
    WHERE UserID = @userID
    AND [SettingsName] = 'Cultural information';

    -- Extract the 'Language' field from JSON, defaulting to 'ENG' if not found
    SET @Lang = COALESCE(JSON_VALUE(@JsonValue, '$.Language'), 'ENG');

    -- Execute the stored procedure to create default categories and locations
    BEGIN TRY
        EXECUTE [dbo].[AddUserCategoriesAndLocations] @userID, @Lang;
    END TRY
    BEGIN CATCH
        -- Optional error handling (e.g., log error)
        PRINT 'Error executing AddUserCategoriesAndLocations procedure';
    END CATCH;
END;
GO


");

            //stp_getbugetsdetails
            migrationBuilder.Sql(@"

CREATE PROCEDURE [dbo].[GetBudgetDetails]
	@PageNumber as int = 1,
	@RowsPage as int = 20
AS
BEGIN
    -- =============================================
    -- Author:        Francis Edgardo Mejía Medina
    -- Create date:   24-09-25
    -- Description:   Retrieves budget details, including total and executed budgets for each month.
	-- Updated date:  2024-10-14
	-- Lastes changes: Rename budgets.month to budgets.title, added RowsPage as parameter
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
	WHERE Budgets.State = 1
    GROUP BY Budgets.Id, Budgets.TotalAllocatedBudget, Budgets.Title, Budgets.CreatedOn, Budgets.UpdatedOn
	ORDER BY Budgets.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END
GO
");

            //stp_getcategorydetails
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetCategoryDetails]
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
	WHERE C.State = 1
    GROUP BY 
        c.Id, c.CreatedOn, c.UpdatedOn, c.Name, c.BudgetLimit
	ORDER BY c.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END

GO
");

            //stp_[GetExpenseDetails]
            migrationBuilder.Sql(@"
CREATE PROCEDURE [dbo].[GetExpenseDetails]
	@BudgetId int,
	@PageNumber as int = 1,
	@RowsPage int = 20
AS
BEGIN
-- =============================================
-- Author:        Francis Mejía 
-- Create date:   2024-09-30
-- Description:   Retrieves budget details, including total and executed budgets for each month.
-- Updated date:  2024-10-14
-- Lastes changes: Added the rowspage parameter, change column budgets.month to budgets.title 
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
	WHERE Budgets.Id = @BudgetId
	Order by Expenses.Id 
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END


GO

");

            //[sp_GetBudgetOverviewJSON]
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[sp_GetBudgetOverviewJSON]
AS 
BEGIN 
    -- =============================================
    -- Author:        FRANCIS MEJIA
    -- Create date:   2024-10-7
    -- Description:   Retrives a summary of the top 5 budgets and the month with the hightest expenses 
    -- Modified On:   2024-10-16
    -- Update:        Fix second json to show the record with the Highest expenses  
    -- =============================================
    Declare @JsonResult nvarchar(max)

    set @JsonResult= (SELECT 
        (SELECT TOP (5)
			B.Id as BudgetId,
            B.Title, 
            B.TotalAllocatedBudget AS Total,
            SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalAllocatedBudget - SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
		WHERE B.State = 1
        GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalAllocatedBudget
        ORDER BY B.CreatedOn DESC
        FOR JSON PATH) AS BudgetTop5,
    
        (SELECT TOP 1 
			B.Id as BudgetId,
            B.Title, 
            B.TotalAllocatedBudget,
            SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalAllocatedBudget - SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
        WHERE YEAR(B.CreatedOn) = YEAR(GETDATE()) 
		AND B.State = 1
        GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalAllocatedBudget
        HAVING SUM(CASE WHEN E.IsExecuted = 1 THEN E.Amount ELSE 0 END) >= 0
		ORDER BY Expenses DESC
        FOR JSON PATH) AS RecordWithHighestExpenses
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)


    SELECT @JsonResult               
END


GO");

            //[stp_CloneShoppingList]
            migrationBuilder.Sql(@"
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
			@CurrentDateString NVARCHAR(50),
			@newId INT;

	-- Start transaction and try block
	BEGIN TRANSACTION;

	BEGIN TRY
		-- Set the current date in string format
		SET @CurrentDateString = CONCAT(YEAR(GETDATE()), '-', MONTH(GETDATE()), '-', DAY(GETDATE()));

		-- Retrieve the description of the shopping list to clone
		SELECT @Description = Description
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
		INSERT INTO Shoppings ([Description]) 
		VALUES (CONCAT(@Description, ' CLONE ', @CurrentDateString));
		
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
");

            //[stp_getfinanceEvaluations]
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[stp_getfinanceEvaluations]
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

GO
");

            //[Stp_getShoppinglist]
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[Stp_getShoppinglist]
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
");

			//[stp_PriceIncreaseCategory]
			migrationBuilder.Sql(@"
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

");

			//[stp_PriceProductIncrease]
			migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[stp_PriceProductIncrease]
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

");

            #endregion

            #region views
            //[vw_PriceIncreaseByBrandAndProduct]
            migrationBuilder.Sql(@"CREATE VIEW [dbo].[vw_PriceIncreaseByBrandAndProduct]
AS
SELECT        Brand, ItemName, SUM(Subtotal) AS TotalByItem, MIN(Subtotal) AS LowestPrice, MAX(Subtotal) AS HighestPrice, (MAX(Subtotal) - MIN(Subtotal)) / NULLIF (MIN(Subtotal), 0) * 100 AS IncreasePercentage
FROM            dbo.ShoppingLists
GROUP BY Brand, ItemName
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = ""(H (1[40] 4[20] 2[20] 3) )""
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = ""(H (1 [50] 4 [25] 3))""
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = ""(H (1 [50] 2 [25] 3))""
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = ""(H (4 [30] 2 [40] 3))""
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = ""(H (1 [56] 3))""
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = ""(H (2 [66] 3))""
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = ""(H (4 [50] 3))""
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = ""(V (3))""
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = ""(H (1[56] 4[18] 2) )""
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = ""(H (1 [75] 4))""
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = ""(H (1[66] 2) )""
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = ""(H (4 [60] 2))""
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = ""(H (1) )""
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = ""(V (4))""
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = ""(V (2))""
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = ""ShoppingLists""
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = """"
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
");

            //[vw_PriceIncreaseByCategoryAndProduct]
            migrationBuilder.Sql(@"CREATE VIEW [dbo].[vw_PriceIncreaseByCategoryAndProduct]
AS
SELECT        c.Name AS Category, sl.ItemName, SUM(sl.Subtotal) AS TotalByItem, MIN(sl.Subtotal) AS LowestPrice, MAX(sl.Subtotal) AS HighestPrice, (MAX(sl.Subtotal) - MIN(sl.Subtotal)) / NULLIF (MIN(sl.Subtotal), 0) 
                         * 100 AS IncreasePercentage
FROM            dbo.ShoppingLists AS sl LEFT OUTER JOIN
                         dbo.Categories AS c ON sl.CategoryId = c.Id
GROUP BY c.Name, sl.ItemName
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = ""(H (1[40] 4[20] 2[20] 3) )""
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = ""(H (1 [50] 4 [25] 3))""
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = ""(H (1 [50] 2 [25] 3))""
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = ""(H (4 [30] 2 [40] 3))""
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = ""(H (1 [56] 3))""
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = ""(H (2 [66] 3))""
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = ""(H (4 [50] 3))""
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = ""(V (3))""
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = ""(H (1[56] 4[18] 2) )""
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = ""(H (1 [75] 4))""
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = ""(H (1[66] 2) )""
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = ""(H (4 [60] 2))""
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = ""(H (1) )""
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = ""(V (4))""
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = ""(V (2))""
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = ""sl""
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = ""c""
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 136
               Right = 474
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = """"
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
");
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //drop store PROCEDURES, TRIGGERS AND VIEWS
            migrationBuilder.Sql(@"
DROP PROCEDURE [dbo].[GetBudgetDetails]
GO

DROP PROCEDURE [dbo].[GetCategoryDetails]
GO

DROP PROCEDURE [dbo].[GetExpenseDetails]
GO

DROP PROCEDURE [dbo].[sp_GetBudgetOverviewJSON]
GO

DROP PROCEDURE [dbo].[stp_CloneShoppingList]
GO

DROP PROCEDURE [dbo].[stp_getfinanceEvaluations]
GO

DROP PROCEDURE [dbo].[Stp_getShoppinglist]
GO

DROP PROCEDURE [dbo].[stp_PriceIncreaseCategory]
GO

DROP PROCEDURE [dbo].[stp_PriceProductIncrease]
GO

DROP PROCEDURE [dbo].[AddUserCategoriesAndLocations]
GO

DROP TRIGGER [dbo].[tg_createUsersSettings]
GO

DROP VIEW [dbo].[vw_PriceIncreaseByBrandAndProduct]
GO

DROP VIEW [dbo].[vw_PriceIncreaseByCategoryAndProduct]
GO

DROP VIEW [dbo].[vw_PriceIncreaseByProducts]
GO
");
        }
    }
}

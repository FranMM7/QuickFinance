USE [master]
GO
/****** Object:  Database [QuickFinanceDB]    Script Date: 10/14/2024 2:51:59 PM ******/
CREATE DATABASE [QuickFinanceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuickFinanceDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\QuickFinanceDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QuickFinanceDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\QuickFinanceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [QuickFinanceDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QuickFinanceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QuickFinanceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [QuickFinanceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QuickFinanceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QuickFinanceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QuickFinanceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QuickFinanceDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [QuickFinanceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QuickFinanceDB] SET  MULTI_USER 
GO
ALTER DATABASE [QuickFinanceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QuickFinanceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QuickFinanceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QuickFinanceDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QuickFinanceDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QuickFinanceDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [QuickFinanceDB] SET QUERY_STORE = OFF
GO
USE [QuickFinanceDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Budgets]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Budgets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TotalBudget] [decimal](18, 2) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_Budgets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[budgetlimit] [decimal](18, 2) NOT NULL,
	[TypeBudget] [bit] NOT NULL,
	[TypeFinanceAnalizis] [bit] NOT NULL,
	[TypeShoppingList] [bit] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenses]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[BudgetId] [int] NOT NULL,
	[PaymentMethodId] [int] NOT NULL,
	[Executed] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinanceDetails]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinanceDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinanceId] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ExpenseType] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_FinanceDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinanceEvaluations]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinanceEvaluations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[Title] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_FinanceEvaluations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethods]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethods](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_PaymentMethods] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShoppingLists]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShoppingLists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShoppingId] [int] NOT NULL,
	[CategoryId] [int] NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[LocationId] [int] NULL,
	[qty] [int] NOT NULL,
	[SubTotal]  AS ([qty]*[Amount]),
 CONSTRAINT [PK_ShoppingLists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shoppings]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shoppings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Shoppings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Expenses_BudgetId]    Script Date: 10/14/2024 2:51:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Expenses_BudgetId] ON [dbo].[Expenses]
(
	[BudgetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Expenses_CategoryId]    Script Date: 10/14/2024 2:51:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Expenses_CategoryId] ON [dbo].[Expenses]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Expenses_PaymentMethodId]    Script Date: 10/14/2024 2:51:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Expenses_PaymentMethodId] ON [dbo].[Expenses]
(
	[PaymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FinanceDetails_CategoryId]    Script Date: 10/14/2024 2:51:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_FinanceDetails_CategoryId] ON [dbo].[FinanceDetails]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FinanceDetails_FinanceId]    Script Date: 10/14/2024 2:51:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_FinanceDetails_FinanceId] ON [dbo].[FinanceDetails]
(
	[FinanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShoppingLists_CategoryId]    Script Date: 10/14/2024 2:51:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShoppingLists_CategoryId] ON [dbo].[ShoppingLists]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShoppingLists_LocationId]    Script Date: 10/14/2024 2:51:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShoppingLists_LocationId] ON [dbo].[ShoppingLists]
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShoppingLists_ShoppingId]    Script Date: 10/14/2024 2:51:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShoppingLists_ShoppingId] ON [dbo].[ShoppingLists]
(
	[ShoppingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Budgets] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF__Categorie__Creat__412EB0B6]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((0.0)) FOR [budgetlimit]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [TypeBudget]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [TypeFinanceAnalizis]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [TypeShoppingList]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF__Expenses__Budget__3D5E1FD2]  DEFAULT ((0)) FOR [BudgetId]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF__Expenses__Paymen__3F466844]  DEFAULT ((0)) FOR [PaymentMethodId]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF__Expenses__Execut__5AEE82B9]  DEFAULT (CONVERT([bit],(0))) FOR [Executed]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF__Expenses__Create__3E52440B]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[FinanceEvaluations] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PaymentMethods] ADD  CONSTRAINT [DF__PaymentMe__Creat__4BAC3F29]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ShoppingLists] ADD  DEFAULT ((1)) FOR [qty]
GO
ALTER TABLE [dbo].[Shoppings] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_Budgets_BudgetId] FOREIGN KEY([BudgetId])
REFERENCES [dbo].[Budgets] ([Id])
GO
ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_Budgets_BudgetId]
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_Categories_CategoryId]
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_PaymentMethods_PaymentMethodId] FOREIGN KEY([PaymentMethodId])
REFERENCES [dbo].[PaymentMethods] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Expenses] CHECK CONSTRAINT [FK_Expenses_PaymentMethods_PaymentMethodId]
GO
ALTER TABLE [dbo].[FinanceDetails]  WITH CHECK ADD  CONSTRAINT [FK_FinanceDetails_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FinanceDetails] CHECK CONSTRAINT [FK_FinanceDetails_Categories_CategoryId]
GO
ALTER TABLE [dbo].[FinanceDetails]  WITH CHECK ADD  CONSTRAINT [FK_FinanceDetails_FinanceEvaluations_FinanceId] FOREIGN KEY([FinanceId])
REFERENCES [dbo].[FinanceEvaluations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FinanceDetails] CHECK CONSTRAINT [FK_FinanceDetails_FinanceEvaluations_FinanceId]
GO
ALTER TABLE [dbo].[ShoppingLists]  WITH CHECK ADD  CONSTRAINT [FK_ShoppingLists_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[ShoppingLists] CHECK CONSTRAINT [FK_ShoppingLists_Categories_CategoryId]
GO
ALTER TABLE [dbo].[ShoppingLists]  WITH CHECK ADD  CONSTRAINT [FK_ShoppingLists_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[ShoppingLists] CHECK CONSTRAINT [FK_ShoppingLists_Locations_LocationId]
GO
ALTER TABLE [dbo].[ShoppingLists]  WITH CHECK ADD  CONSTRAINT [FK_ShoppingLists_Shoppings_ShoppingId] FOREIGN KEY([ShoppingId])
REFERENCES [dbo].[Shoppings] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoppingLists] CHECK CONSTRAINT [FK_ShoppingLists_Shoppings_ShoppingId]
GO
/****** Object:  StoredProcedure [dbo].[GetBudgetDetails]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
        Budgets.TotalBudget, 
        SUM(CASE WHEN Expenses.Executed = 1 THEN Expenses.Amount ELSE 0 END) AS ExecutedBudget,
        Budgets.Title,
        CASE 
            WHEN Budgets.UpdatedOn IS NULL THEN Budgets.CreatedOn 
            ELSE Budgets.UpdatedOn 
        END AS ModifiedOn
    FROM Budgets
    LEFT OUTER JOIN Expenses ON Expenses.BudgetId = Budgets.Id
    GROUP BY Budgets.Id, Budgets.TotalBudget, Budgets.Title, Budgets.CreatedOn, Budgets.UpdatedOn
	ORDER BY Budgets.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END
GO
/****** Object:  StoredProcedure [dbo].[GetCategoryDetails]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCategoryDetails]
	@PageNumber as int = 1,
	@RowsPage int = 20
AS
BEGIN
	-- =============================================
	-- Author:		Francis E Mejia
	-- Create date: 24-09-25
	-- Description:	Will display basic information about the category
	-- Updated date:  2024-10-14
	-- Lastes changes: Added Rows Page as parameter
    -- =============================================
	SET NOCOUNT ON;

	--Declare @RowsPage int = 20

    SELECT 
        c.Id,
        c.Name,
		c.BudgetLimit,
		c.CreatedOn,
		C.UpdatedOn,
        COUNT(e.Id) AS ExpenseCount,
        CAST(ISNULL(SUM(CASE WHEN e.Executed = 1 THEN e.Amount END), 0) AS decimal(18,2)) AS TotalExpended,
        ModifiedOn = CAST( CASE WHEN C.UpdatedOn IS NULL THEN C.CreatedOn ELSE C.UpdatedOn END AS DATE)
    FROM 
        Categories c
    LEFT JOIN 
        Expenses e ON e.CategoryId = c.Id
    GROUP BY 
        c.Id, c.CreatedOn, c.UpdatedOn, c.Name, c.BudgetLimit
	ORDER BY c.Id
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;

END
GO
/****** Object:  StoredProcedure [dbo].[GetExpenseDetails]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
        Expenses.DueDate, 
        Expenses.CategoryId, 
        Categories.Name AS Category, 
        Expenses.BudgetId, 
        Budgets.Title, 
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
	Order by Expenses.Id 
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetBudgetOverviewJSON]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetBudgetOverviewJSON]
AS 
BEGIN 
    -- =============================================
    -- Author:        FRANCIS MEJIA
    -- Create date:   2024-10-7
    -- Description:   Retrives a summary of the top 5 budgets and the month with the hightest expenses 
    -- Modified On:   2024-10-14
    -- Update:        column b.month was rename to b.title 
    -- =============================================
    Declare @JsonResult nvarchar(max)

    set @JsonResult= (SELECT 
        (SELECT TOP (5)
			B.Id as BudgetId,
            B.Title, 
            B.TotalBudget,
            SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalBudget - SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
        GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalBudget
        ORDER BY B.CreatedOn DESC
        FOR JSON PATH) AS BudgetTop5,
    
        (SELECT TOP 1 
			B.Id as BudgetId,
            B.Title, 
            B.TotalBudget,
            SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Expenses,
            B.TotalBudget - SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) AS Saving
        FROM Budgets AS B 
        LEFT OUTER JOIN Expenses AS E 
            ON E.BudgetId = B.Id
        WHERE YEAR(B.CreatedOn) = YEAR(GETDATE()) 
        GROUP BY B.Id, B.Title, B.CreatedOn, B.TotalBudget
        HAVING SUM(CASE WHEN E.EXECUTED = 1 THEN E.Amount ELSE 0 END) >= 0
        ORDER BY B.CreatedOn DESC
        FOR JSON PATH) AS MonthWithHighestExpenses
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)


    SELECT @JsonResult               
END
GO
/****** Object:  StoredProcedure [dbo].[stp_getfinanceEvaluations]    Script Date: 10/14/2024 2:51:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stp_getfinanceEvaluations]
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
			CASE WHEN FD.ExpenseType = 1 THEN 'IMPORTANT' 
				 WHEN FD.ExpenseType = 2 THEN 'Ghost Expense' 
				 WHEN FD.ExpenseType = 3 THEN 'Ant Expense' 
				 WHEN FD.ExpenseType = 4 THEN 'Vampire Expense' 
			ELSE 'OTHER/ND' END ExpenseType,
			Amount
	FROM FinanceEvaluations FE
	LEFT OUTER JOIN FinanceDetails FD 
		ON FE.Id = FD.FinanceId
	LEFT OUTER JOIN Categories C
		ON FD.CategoryId = C.Id
	ORDER BY FE.Id, FD.Id DESC
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END
GO
/****** Object:  StoredProcedure [dbo].[Stp_getShoppinglist]    Script Date: 10/14/2024 2:51:59 PM ******/
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
			SL.Description AS Item, 
			sl.qty,
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
	ORDER BY S.Id, SL.Id
	OFFSET (@PageNumber-1)*@RowsPage ROWS
	FETCH NEXT @RowsPage ROWS Only;

END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 = Important
2 = Ghost Expense
3 = Ant Expense
4 = Vampire Expense' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FinanceDetails', @level2type=N'COLUMN',@level2name=N'ExpenseType'
GO
USE [master]
GO
ALTER DATABASE [QuickFinanceDB] SET  READ_WRITE 
GO

USE [master]
GO
/****** Object:  Database [QuickFinanceDB]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE DATABASE [QuickFinanceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuickFinanceDB', FILENAME = N'D:\Program Files\MSSQL\MSSQL16.SQLEXPRESS\MSSQL\DATA\QuickFinanceDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QuickFinanceDB_log', FILENAME = N'D:\Program Files\MSSQL\MSSQL16.SQLEXPRESS\MSSQL\DATA\QuickFinanceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
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
ALTER DATABASE [QuickFinanceDB] SET RECOVERY FULL 
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
/****** Object:  Table [dbo].[ShoppingLists]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShoppingLists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShoppingId] [int] NOT NULL,
	[CategoryId] [int] NULL,
	[ItemName] [nvarchar](max) NOT NULL,
	[Brand] [nvarchar](50) NULL,
	[Quantity] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[LocationId] [int] NULL,
	[Subtotal]  AS ([Quantity]*[Amount]),
 CONSTRAINT [PK_ShoppingLists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_PriceIncreaseByProducts]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_PriceIncreaseByProducts]
AS
SELECT        ItemName, SUM(Subtotal) AS TotalByItem, MIN(Subtotal) AS LowestPrice, MAX(Subtotal) AS HighestPrice, (MAX(Subtotal) - MIN(Subtotal)) / NULLIF (MIN(Subtotal), 0) * 100 AS IncreasePercentage
FROM            dbo.ShoppingLists
GROUP BY ItemName
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[BudgetLimit] [decimal](18, 2) NOT NULL,
	[TypeBudget] [bit] NOT NULL,
	[TypeFinanceAnalizis] [bit] NOT NULL,
	[TypeShoppingList] [bit] NOT NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_PriceIncreaseByCategoryAndProduct]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_PriceIncreaseByCategoryAndProduct]
AS
SELECT        c.Name AS Category, sl.ItemName, SUM(sl.Subtotal) AS TotalByItem, MIN(sl.Subtotal) AS LowestPrice, MAX(sl.Subtotal) AS HighestPrice, (MAX(sl.Subtotal) - MIN(sl.Subtotal)) / NULLIF (MIN(sl.Subtotal), 0) 
                         * 100 AS IncreasePercentage
FROM            dbo.ShoppingLists AS sl LEFT OUTER JOIN
                         dbo.Categories AS c ON sl.CategoryId = c.Id
GROUP BY c.Name, sl.ItemName
GO
/****** Object:  View [dbo].[vw_PriceIncreaseByBrandAndProduct]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_PriceIncreaseByBrandAndProduct]
AS
SELECT        Brand, ItemName, SUM(Subtotal) AS TotalByItem, MIN(Subtotal) AS LowestPrice, MAX(Subtotal) AS HighestPrice, (MAX(Subtotal) - MIN(Subtotal)) / NULLIF (MIN(Subtotal), 0) * 100 AS IncreasePercentage
FROM            dbo.ShoppingLists
GROUP BY Brand, ItemName
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/6/2024 10:58:36 PM ******/
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
/****** Object:  Table [dbo].[Budgets]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Budgets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TotalAllocatedBudget] [decimal](18, 2) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Budgets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenses]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[ExpenseDueDate] [datetime2](7) NULL,
	[CategoryId] [int] NOT NULL,
	[BudgetId] [int] NOT NULL,
	[PaymentMethodId] [int] NOT NULL,
	[IsExecuted] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinanceDetails]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinanceDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinanceId] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ExpenseCategory] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_FinanceDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinanceEvaluations]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinanceEvaluations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[Title] [nvarchar](max) NOT NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_FinanceEvaluations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[Name] [nvarchar](max) NOT NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethods]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethods](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentMethodName] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_PaymentMethods] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shoppings]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shoppings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[Description] [nvarchar](max) NOT NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_Shoppings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/6/2024 10:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](450) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Expenses_BudgetId]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_Expenses_BudgetId] ON [dbo].[Expenses]
(
	[BudgetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Expenses_CategoryId]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_Expenses_CategoryId] ON [dbo].[Expenses]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Expenses_PaymentMethodId]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_Expenses_PaymentMethodId] ON [dbo].[Expenses]
(
	[PaymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FinanceDetails_CategoryId]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_FinanceDetails_CategoryId] ON [dbo].[FinanceDetails]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FinanceDetails_FinanceId]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_FinanceDetails_FinanceId] ON [dbo].[FinanceDetails]
(
	[FinanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShoppingLists_CategoryId]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShoppingLists_CategoryId] ON [dbo].[ShoppingLists]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShoppingLists_LocationId]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShoppingLists_LocationId] ON [dbo].[ShoppingLists]
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShoppingLists_ShoppingId]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShoppingLists_ShoppingId] ON [dbo].[ShoppingLists]
(
	[ShoppingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_Email]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_Username]    Script Date: 11/6/2024 10:58:36 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username] ON [dbo].[Users]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Budgets] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Budgets] ADD  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF__Categorie__Creat__412EB0B6]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((0.0)) FOR [BudgetLimit]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [TypeBudget]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [TypeFinanceAnalizis]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(0))) FOR [TypeShoppingList]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF__Expenses__Budget__3D5E1FD2]  DEFAULT ((0)) FOR [BudgetId]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF__Expenses__Paymen__3F466844]  DEFAULT ((0)) FOR [PaymentMethodId]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF__Expenses__Execut__5AEE82B9]  DEFAULT (CONVERT([bit],(0))) FOR [IsExecuted]
GO
ALTER TABLE [dbo].[Expenses] ADD  CONSTRAINT [DF__Expenses__Create__3E52440B]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[FinanceEvaluations] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[FinanceEvaluations] ADD  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[PaymentMethods] ADD  CONSTRAINT [DF__PaymentMe__Creat__4BAC3F29]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[PaymentMethods] ADD  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[Shoppings] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Shoppings] ADD  CONSTRAINT [DF__Shoppings__State__0697FACD]  DEFAULT ((1)) FOR [State]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
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
/****** Object:  StoredProcedure [dbo].[GetBudgetDetails]    Script Date: 11/6/2024 10:58:36 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GetCategoryDetails]    Script Date: 11/6/2024 10:58:36 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GetExpenseDetails]    Script Date: 11/6/2024 10:58:36 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_GetBudgetOverviewJSON]    Script Date: 11/6/2024 10:58:36 PM ******/
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


GO
/****** Object:  StoredProcedure [dbo].[stp_CloneShoppingList]    Script Date: 11/6/2024 10:58:36 PM ******/
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
/****** Object:  StoredProcedure [dbo].[stp_getfinanceEvaluations]    Script Date: 11/6/2024 10:58:36 PM ******/
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
			CASE WHEN FD.ExpenseCategory = 1 THEN 'IMPORTANT' 
				 WHEN FD.ExpenseCategory = 2 THEN 'Ghost Expense' 
				 WHEN FD.ExpenseCategory = 3 THEN 'Ant Expense' 
				 WHEN FD.ExpenseCategory = 4 THEN 'Vampire Expense' 
			ELSE 'OTHER/ND' END ExpenseType,
			Amount
	FROM FinanceEvaluations FE
	LEFT OUTER JOIN FinanceDetails FD 
		ON FE.Id = FD.FinanceId
	LEFT OUTER JOIN Categories C
		ON FD.CategoryId = C.Id
	WHERE FE.State=1
	ORDER BY FE.Id, FD.Id DESC
	OFFSET (@PageNumber-1)*@RowsPage Rows
	FETCH NEXT @RowsPage ROWS Only;
END

GO
/****** Object:  StoredProcedure [dbo].[Stp_getShoppinglist]    Script Date: 11/6/2024 10:58:36 PM ******/
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
/****** Object:  StoredProcedure [dbo].[stp_PriceIncreaseCategory]    Script Date: 11/6/2024 10:58:36 PM ******/
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
/****** Object:  StoredProcedure [dbo].[stp_PriceProductIncrease]    Script Date: 11/6/2024 10:58:36 PM ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 = Important
2 = Ghost Expense
3 = Ant Expense
4 = Vampire Expense' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FinanceDetails', @level2type=N'COLUMN',@level2name=N'ExpenseCategory'
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
         Begin Table = "sl"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_PriceIncreaseByProducts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_PriceIncreaseByProducts'
GO
USE [master]
GO
ALTER DATABASE [QuickFinanceDB] SET  READ_WRITE 
GO

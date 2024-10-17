USE QuickFinanceDB;
GO

SET XACT_ABORT ON; -- Automatically roll back the transaction on any error

BEGIN TRY
    BEGIN TRANSACTION T1;
    
    -- Clean up Expenses table
    DELETE FROM Expenses;  -- Using DELETE to avoid issues with foreign keys
    DBCC CHECKIDENT ('Expenses', RESEED, 0);  -- Reset the identity seed for Expenses

    -- Clean up Budgets table
    DELETE FROM Budgets;  -- Using DELETE to avoid issues with foreign keys
    DBCC CHECKIDENT ('Budgets', RESEED, 0);  -- Reset the identity seed for Budgets

    -- Clean up PaymentMethods table
    DELETE FROM PaymentMethods;  -- Using DELETE to avoid issues with foreign keys
    DBCC CHECKIDENT ('PaymentMethods', RESEED, 0);  -- Reset the identity seed for PaymentMethods	
	
	-- Clean up Shopping List table
	DELETE FROM [dbo].[ShoppingLists]
	DBCC CHECKIDENT ('ShoppingLists', RESEED, 0);  -- Reset the identity seed for ShoppingLists
	
	-- Clean up Shopping table
	DELETE FROM [dbo].[Shoppings]
	DBCC CHECKIDENT ('Shoppings', RESEED, 0);  -- Reset the identity seed for Shoppings

	-- Clean up Shopping List table
	DELETE FROM [dbo].[Locations]
	DBCC CHECKIDENT ('Locations', RESEED, 0);  -- Reset the identity seed for ShoppingLists

	-- Clean up Finance Details table 
	DELETE FROM FinanceDetails
	DBCC CHECKIDENT ('FinanceDetails', RESEED, 0);  -- Reset the identity seed for FinanceDetails
	
	-- Clean up Finance Evaluation table 
	DELETE FROM FinanceEvaluations
	DBCC CHECKIDENT ('FinanceEvaluations', RESEED, 0);  -- Reset the identity seed for FinanceEvaluations

    -- Clean up Categories table
    DELETE FROM Categories;  -- Using DELETE to avoid issues with foreign keys
    DBCC CHECKIDENT ('Categories', RESEED, 0);  -- Reset the identity seed for Categories

	-- Clean up Users table
	--DELETE FROM Users

    -- If everything is successful, commit the transaction
    COMMIT TRANSACTION T1;

END TRY
BEGIN CATCH
    -- Rollback transaction in case of an error
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION T1;
    END

    -- Return the error information
    SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_MESSAGE() AS ErrorMessage;
END CATCH;

GO

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

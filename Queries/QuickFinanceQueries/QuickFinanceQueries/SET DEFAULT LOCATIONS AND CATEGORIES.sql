use [QuickFinanceDB-Copy]

select *
from Locations

select *
from Categories

Declare @userID nvarchar(450) = '0104021d-1b1f-41f3-8be5-68d0f08f23d1'
Declare @lang nvarchar(50) = 'SPA'
Begin tran

	IF @lang = 'ENG'
        BEGIN
            INSERT INTO Locations (Name, UserId,State) VALUES ('Local-Market', @userID,1);
        END
        ELSE IF @lang = 'SPA'
        BEGIN
            INSERT INTO Locations (Name, UserId, State) VALUES ('Mercado-Local', @userID,1);
        END

        -- Insert Category records with language-specific names
        IF @lang = 'ENG'
        BEGIN
            INSERT INTO Categories (Name, BudgetLimit, TypeBudget, TypeShoppingList, TypeFinanceAnalizis, UserId, State)
            VALUES 
                ('Food', 0, 1, 0, 1, @userID,1),
                ('Transport', 0, 1, 0, 0,  @userID,1),
                ('Entertainment', 0, 1, 1, 1,  @userID,1),
                ('Dairy', 0, 1, 1, 0,  @userID,1),
                ('Meats', 0, 1, 1, 0, @userID,1),
                ('Cleaning', 0, 1, 1, 1,  @userID,1),
                ('Utilities', 0, 1, 1, 1,  @userID,1),
                ('Health', 0, 1, 1, 1,  @userID,1),
				('House', 0, 1, 1, 1,  @userID,1);
        END
        ELSE IF @lang = 'SPA'
        BEGIN
            INSERT INTO Categories (Name, BudgetLimit, TypeBudget, TypeShoppingList, TypeFinanceAnalizis, UserId,State)
            VALUES 
                ('Alimentos', 0, 1, 0, 1, @userID,1),
                ('Transporte', 0, 1, 0, 0,  @userID,1),
                ('Entretenimiento', 0, 1, 1, 1,  @userID,1),
                ('Lácteos', 0, 1, 1, 0,  @userID,1),
                ('Carnes', 0, 1, 1, 0,  @userID,1),
                ('Limpieza', 0, 1, 1, 1,  @userID,1),
                ('Servicios Públicos', 0, 1, 1, 1, @userID,1),
                ('Salud', 0, 1, 1, 1,  @userID,1),
				('Casa', 0, 1, 1, 1,  @userID,1);
        END
		
		SELECT *
		FROM Locations
		WHERE UserId = @userID

		SELECT *
		FROM Categories
		WHERE UserId=@userID
rollback tran
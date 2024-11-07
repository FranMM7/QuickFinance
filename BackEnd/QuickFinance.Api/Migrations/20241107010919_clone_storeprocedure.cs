using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class clone_storeprocedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROCEDURE stp_CloneShoppingList
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP PROCEDURE stp_CloneShoppingList");
        }
    }
}

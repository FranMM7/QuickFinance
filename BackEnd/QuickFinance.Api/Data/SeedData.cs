using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using System;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new FinanceContext(
            serviceProvider.GetRequiredService<DbContextOptions<FinanceContext>>());

        // Check if the database has already been seeded
        if (context.Budgets.Any())
        {
            return;   // DB has been seeded
        }

        // Seed Categories with new flags
        var categories = new[]
        {
            new Category { Name = "Food", budgetlimit = 300, TypeBudget = true, TypeShoppingList = false, TypeFinanceAnalizis = false },
            new Category { Name = "Transport", budgetlimit = 20.2M, TypeBudget = true, TypeShoppingList = false, TypeFinanceAnalizis = false },
            new Category { Name = "Entertainment", budgetlimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = false },
            new Category { Name = "Dairy", budgetlimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = false },
            new Category { Name = "Meats", budgetlimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = false },
            new Category { Name = "Cleaning", budgetlimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = false },
            new Category { Name = "Utilities", budgetlimit = 200, TypeBudget = true, TypeShoppingList = false, TypeFinanceAnalizis = true },
            new Category { Name = "Health", budgetlimit = 150, TypeBudget = true, TypeShoppingList = false, TypeFinanceAnalizis = true }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges(); // Save categories first

        // Seed Payment Methods
        var paymentMethods = new[]
        {
            new PaymentMethod { Name = "Credit Card" },
            new PaymentMethod { Name = "Cash" },
            new PaymentMethod { Name = "Debit Card" },
            new PaymentMethod { Name = "Bank Transfer" }
        };

        context.PaymentMethods.AddRange(paymentMethods);
        context.SaveChanges(); // Save payment methods

        // Seed Budgets (renamed from 'Month' to 'Title')
        var budgets = new[]
        {
            new Budget { Title = "January Budget" },
            new Budget { Title = "February Budget"}
        };

        context.Budgets.AddRange(budgets);
        context.SaveChanges(); // Save budgets

        // Seed Expenses
        var expenses = new[]
        {
            new Expense { Description = "Groceries", Amount = 150.00M, BudgetId = budgets[0].Id, CategoryId = categories[0].Id, PaymentMethodId = paymentMethods[0].Id, DueDate = DateTime.Now, Executed = false },
            new Expense { Description = "Gas", Amount = 50.00M, BudgetId = budgets[0].Id, CategoryId = categories[1].Id, PaymentMethodId = paymentMethods[1].Id, DueDate = DateTime.Now, Executed = false },
            new Expense { Description = "Movie Tickets", Amount = 30.00M, BudgetId = budgets[1].Id, CategoryId = categories[2].Id, PaymentMethodId = paymentMethods[2].Id, DueDate = DateTime.Now, Executed = true }
        };

        context.Expenses.AddRange(expenses);
        context.SaveChanges(); // Save expenses

        // Seed FinanceEvaluation and FinanceDetails
        var financeEvaluations = new[]
        {
            new FinanceEvaluation { Title = "January Finance Analysis" },
            new FinanceEvaluation { Title = "February Finance Analysis" }
        };

        context.FinanceEvaluations.AddRange(financeEvaluations);
        context.SaveChanges(); //Save finance evaluations

        var financeDetails = new[]
        {
            new FinanceDetail { FinanceId = financeEvaluations[0].Id, Description = "Rent", Amount = 500, CategoryId = categories[3].Id, ExpenseType = 1 },
            new FinanceDetail { FinanceId = financeEvaluations[0].Id, Description = "Electricity Bill", Amount = 100, CategoryId = categories[3].Id, ExpenseType = 4 },
            new FinanceDetail { FinanceId = financeEvaluations[1].Id, Description = "Gym Membership", Amount = 50, CategoryId = categories[4].Id, ExpenseType = 3 }
        };

        context.FinanceDetails.AddRange(financeDetails);
        context.SaveChanges(); // Save finance details

        // Seed Locations 
        var locations = new[]
        {
            new Locations { CreatedOn = DateTime.Now, Name = "Local-Market" },
            new Locations { CreatedOn = DateTime.Now, Name = "Walmart" }
        };
        context.Locations.AddRange(locations);
        context.SaveChanges(); // Save locataions

        // Seed Shopping and ShoppingList
        var shoppings = new[]
        {
            new Shopping { Description = "Groceries Shopping" },
            new Shopping { Description = "Clothes Shopping" }
        };

        context.Shoppings.AddRange(shoppings);
        context.SaveChanges(); //save shopping

        var shoppingLists = new[]
        {
            new ShoppingList { ShoppingId = shoppings[0].Id, CategoryId = categories[0].Id, LocationId = locations[0].Id, Description = "Weekly groceries",qty=1, Amount = 100 },
            new ShoppingList { ShoppingId = shoppings[1].Id, CategoryId = categories[2].Id, LocationId = locations[0].Id, Description = "New outfit",qty=2, Amount = 75 }
        };

        context.ShoppingLists.AddRange(shoppingLists);
        context.SaveChanges(); // Save shopping lists
    }
}

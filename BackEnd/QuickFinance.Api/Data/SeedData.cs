using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuickFinance.Api;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
    {
        using var context = new FinanceContext(
            serviceProvider.GetRequiredService<DbContextOptions<FinanceContext>>());

        // Retrieve admin user ID
        var adminUser = await userManager.FindByNameAsync("admin");

        if (adminUser == null)
        {
            // Admin user not found, abort seeding
            return;
        }

        var adminUserId = adminUser.Id;

        // Seed setting
        if (!await context.Settings.AnyAsync())
        {
            var culturalInfo = new
            {
                Language = "ENG",
                Currency = "USD-DOLLAR",
                CurrencySymbol = "$"
            };

            var settings = new[]
            {
                new Settings
                {
                    SettingsName = "Cultural information",
                    JsonValue = JsonSerializer.Serialize(culturalInfo),  // Serializes object to JSON string
                    UserId = adminUserId
                }
            };

            await context.Settings.AddRangeAsync(settings);
            await context.SaveChangesAsync();
        }


        // Seed Categories
        if (!context.Categories.Any())
        {
            var categories = new[]
            {
                new Category { Name = "Food", BudgetLimit = 300, TypeBudget = true, TypeShoppingList = false, TypeFinanceAnalizis = true, UserId = adminUserId },
                new Category { Name = "Transport", BudgetLimit = 20.2M, TypeBudget = true, TypeShoppingList = false, TypeFinanceAnalizis = false, UserId = adminUserId },
                new Category { Name = "Entertainment", BudgetLimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = true, UserId = adminUserId },
                new Category { Name = "Dairy", BudgetLimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = false, UserId = adminUserId },
                new Category { Name = "Meats", BudgetLimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = false, UserId = adminUserId },
                new Category { Name = "Cleaning", BudgetLimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = true, UserId = adminUserId },
                new Category { Name = "Utilities", BudgetLimit = 200, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = true, UserId = adminUserId },
                new Category { Name = "Health", BudgetLimit = 150, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = true, UserId = adminUserId },
                new Category { Name = "House", BudgetLimit = 150, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = true, UserId = adminUserId },
            };

            context.Categories.AddRange(categories);
            context.SaveChanges(); // Save categories first
        }

        // Seed Payment Methods
        if (!context.PaymentMethods.Any())
        {
            var paymentMethods = new[]
            {
                new PaymentMethod { PaymentMethodName = "Credit Card" },
                new PaymentMethod { PaymentMethodName = "Cash" },
                new PaymentMethod { PaymentMethodName = "Debit Card" },
                new PaymentMethod { PaymentMethodName = "Bank Transfer" }
            };

            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges(); // Save payment methods
        }


        // Seed Budgets
        if (!context.Budgets.Any())
        {
            var budgets = new[]
            {
                new Budget { Title = "January Budget", TotalAllocatedBudget = 1500, UserId = adminUserId },
                new Budget { Title = "February Budget", TotalAllocatedBudget = 15000, UserId = adminUserId }
            };

            // Save budgets first to generate IDs
            context.Budgets.AddRange(budgets);
            context.SaveChanges();

            // Retrieve the saved budgets with their IDs
            var budgetsList = context.Budgets.ToList();

            var categories = context.Categories.ToList();
            var paymentMethods = context.PaymentMethods.ToList();

            var expenses = new[]
            {
                new Expense { Description = "Groceries", Amount = 150.00M, BudgetId = budgetsList[0].Id, CategoryId = categories[0].Id, PaymentMethodId = paymentMethods[0].Id, ExpenseDueDate = DateTime.Now, IsExecuted = false },
                new Expense { Description = "Gas", Amount = 50.00M, BudgetId = budgetsList[0].Id, CategoryId = categories[1].Id, PaymentMethodId = paymentMethods[1].Id, ExpenseDueDate = DateTime.Now, IsExecuted = false },
                new Expense { Description = "Movie Tickets", Amount = 30.00M, BudgetId = budgetsList[1].Id, CategoryId = categories[2].Id, PaymentMethodId = paymentMethods[2].Id, ExpenseDueDate = DateTime.Now, IsExecuted = true }
            };

            context.Expenses.AddRange(expenses);
            context.SaveChanges(); // Save expenses
        }


        // Seed FinanceEvaluations and related data
        if (!context.FinanceEvaluations.Any())
        {
            var financeEvaluations = new[]
            {
                new FinanceEvaluation { Title = "January 2024 Analysis", UserId = adminUserId },
                new FinanceEvaluation { Title = "February 2024 Analysis", UserId = adminUserId }
            };

            context.FinanceEvaluations.AddRange(financeEvaluations);
            context.SaveChanges();

            // Retrieve the saved financeEvaluations with IDs
            var feList = context.FinanceEvaluations.ToList();

            // Seed FinanceDetails and FinanceIncomes after saving FinanceEvaluations
            var categories = context.Categories.ToList();

            var financeDetails = new[]
            {
                new FinanceDetail { FinanceEvaluationId = feList[0].Id, Description = "Rent", Amount = 500, CategoryId = categories[7].Id, ExpenseCategory = 1 },
                new FinanceDetail { FinanceEvaluationId = feList[0].Id, Description = "Electricity Bill", Amount = 100, CategoryId = categories[7].Id, ExpenseCategory = 4 },
                new FinanceDetail { FinanceEvaluationId = feList[1].Id, Description = "Gym Membership", Amount = 50, CategoryId = categories[8].Id, ExpenseCategory = 3 }
            };

            var financeIncomes = new[]
            {
                new FinanceIncome { FinanceId = feList[0].Id, Description = "Monthly Salary Income", Amount = 1500 },
                new FinanceIncome { FinanceId = feList[1].Id, Description = "Monthly Salary Income", Amount = 1500 }
            };

            // Add and save FinanceDetails and FinanceIncomes
            context.FinanceDetails.AddRange(financeDetails);
            context.FinanceIncomes.AddRange(financeIncomes);
            context.SaveChanges();

            // Calculate totals for each FinanceEvaluation and update TotalExpense and TotalIncome
            foreach (var evaluation in feList)
            {
                // Sum expenses
                var totalExpense = context.FinanceDetails
                    .Where(d => d.FinanceEvaluationId == evaluation.Id)
                    .Sum(d => d.Amount);

                // Sum incomes
                var totalIncome = context.FinanceIncomes
                    .Where(i => i.FinanceId == evaluation.Id)
                    .Sum(i => i.Amount);

                // Update the evaluation totals
                evaluation.TotalExpenses = totalExpense;
                evaluation.TotalIncomes = totalIncome;
            }

            // Save the updated totals to the database
            context.SaveChanges();
        }


        // Seed Locations
        if (!context.Locations.Any())
        {
            var locations = new[]
            {
                new Locations { CreatedOn = DateTime.Now, Name = "Local-Market", UserId=adminUserId },
                new Locations { CreatedOn = DateTime.Now, Name = "Walmart", UserId=adminUserId }
            };
            context.Locations.AddRange(locations);
            context.SaveChanges(); // Save locations
        }

        // Seed Shoppings and ShoppingLists
        if (!context.Shoppings.Any())
        {
            var categories = context.Categories.ToList();
            var locations = context.Locations.ToList();

            var shoppings = new[]
            {
                new Shopping { Description = "Groceries Shopping", UserId=adminUserId },
                new Shopping { Description = "Clothes Shopping", UserId=adminUserId }
            };

            context.Shoppings.AddRange(shoppings);
            context.SaveChanges();

            var shoppingLists = new[]
            {
                new ShoppingList { ShoppingId = shoppings[0].Id, CategoryId = categories[0].Id, LocationId = locations[0].Id, Brand = "Leyde", ItemName = "Milk", Quantity = 1, Amount = 100 },
                new ShoppingList { ShoppingId = shoppings[1].Id, CategoryId = categories[2].Id, LocationId = locations[0].Id, Brand = "Berska", ItemName = "New outfit", Quantity = 2, Amount = 75 }
            };

            context.ShoppingLists.AddRange(shoppingLists);
            context.SaveChanges(); // Save shopping lists
        }
    }
}

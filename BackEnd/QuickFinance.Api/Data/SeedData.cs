using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using System;
using System.Linq;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new FinanceContext(
            serviceProvider.GetRequiredService<DbContextOptions<FinanceContext>>());

        // Seed Users
        if (!context.Users.Any())
        {

            var usrManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var user = new IdentityUser { UserName = "admin", Email = "admin@example.com" };
            var result = await usrManager.CreateAsync(user, "AdminPassword123");

            //var users = new[]
            //{
            //    new User
            //    {
            //        Username = "admin",
            //        Email = "admin@example.com",
            //        Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"), // hashed password
            //        CreatedAt = DateTime.UtcNow,
            //        UpdatedAt = DateTime.UtcNow
            //    },
            //    new User
            //    {
            //        Username = "user1",
            //        Email = "user1@example.com",
            //        Password = BCrypt.Net.BCrypt.HashPassword("User@123"), // hashed password
            //        CreatedAt = DateTime.UtcNow,
            //        UpdatedAt = DateTime.UtcNow
            //    }
            //};

            //context.Users.AddRange(users);
            //context.SaveChanges(); // Save seeded users
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Check if admin user exists
        if (userManager.FindByNameAsync("admin").Result == null)
        {
            var user = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com"
            };
            var result = userManager.CreateAsync(user, "AdminPassword123").Result;

            if (result.Succeeded)
            {
                // Assign role or additional seeding logic
            }
        }

            // Seed Categories
            if (!context.Categories.Any())
        {
            var categories = new[]
            {
                new Category { Name = "Food", BudgetLimit = 300, TypeBudget = true, TypeShoppingList = false, TypeFinanceAnalizis = true },
                new Category { Name = "Transport", BudgetLimit = 20.2M, TypeBudget = true, TypeShoppingList = false, TypeFinanceAnalizis = false },
                new Category { Name = "Entertainment", BudgetLimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = true },
                new Category { Name = "Dairy", BudgetLimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = false },
                new Category { Name = "Meats", BudgetLimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = false },
                new Category { Name = "Cleaning", BudgetLimit = 100, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = true },
                new Category { Name = "Utilities", BudgetLimit = 200, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = true },
                new Category { Name = "Health", BudgetLimit = 150, TypeBudget = true, TypeShoppingList = true, TypeFinanceAnalizis = true }
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
                new Budget { Title = "January Budget", TotalAllocatedBudget = 1500 },
                new Budget { Title = "February Budget", TotalAllocatedBudget = 15000 }
            };

            context.Budgets.AddRange(budgets);
            context.SaveChanges(); // Save budgets
        }

        // Seed Expenses
        if (!context.Expenses.Any())
        {
            var budgets = context.Budgets.ToList();
            var categories = context.Categories.ToList();
            var paymentMethods = context.PaymentMethods.ToList();

            var expenses = new[]
            {
                new Expense { Description = "Groceries", Amount = 150.00M, BudgetId = budgets[0].Id, CategoryId = categories[0].Id, PaymentMethodId = paymentMethods[0].Id, ExpenseDueDate = DateTime.Now, IsExecuted = false },
                new Expense { Description = "Gas", Amount = 50.00M, BudgetId = budgets[0].Id, CategoryId = categories[1].Id, PaymentMethodId = paymentMethods[1].Id, ExpenseDueDate = DateTime.Now, IsExecuted = false },
                new Expense { Description = "Movie Tickets", Amount = 30.00M, BudgetId = budgets[1].Id, CategoryId = categories[2].Id, PaymentMethodId = paymentMethods[2].Id, ExpenseDueDate = DateTime.Now, IsExecuted = true }
            };

            context.Expenses.AddRange(expenses);
            context.SaveChanges(); // Save expenses
        }

        // Seed FinanceEvaluations and related data
        if (!context.FinanceEvaluations.Any())
        {
            var financeEvaluations = new[]
            {
                new FinanceEvaluation { Title = "January Finance Analysis" },
                new FinanceEvaluation { Title = "February Finance Analysis" }
            };

            context.FinanceEvaluations.AddRange(financeEvaluations);
            context.SaveChanges();

            // Seed FinanceDetails and FinanceIncomes after saving FinanceEvaluations
            var categories = context.Categories.ToList();

            var financeDetails = new[]
            {
                new FinanceDetail { FinanceId = financeEvaluations[0].Id, Description = "Rent", Amount = 500, CategoryId = categories[3].Id, ExpenseCategory = 1 },
                new FinanceDetail { FinanceId = financeEvaluations[0].Id, Description = "Electricity Bill", Amount = 100, CategoryId = categories[3].Id, ExpenseCategory = 4 },
                new FinanceDetail { FinanceId = financeEvaluations[1].Id, Description = "Gym Membership", Amount = 50, CategoryId = categories[4].Id, ExpenseCategory = 3 }
            };

            var financeIncomes = new[]
            {
                new FinanceIncome { FinanceId = financeEvaluations[0].Id, Description = "Monthly Salary Income", Amount = 1500 },
                new FinanceIncome { FinanceId = financeEvaluations[1].Id, Description = "Monthly Salary Income", Amount = 1500 }
            };

            // Add and save FinanceDetails and FinanceIncomes
            context.FinanceDetails.AddRange(financeDetails);
            context.FinanceIncomes.AddRange(financeIncomes);
            context.SaveChanges();
        }

        // Seed Locations
        if (!context.Locations.Any())
        {
            var locations = new[]
            {
                new Locations { CreatedOn = DateTime.Now, Name = "Local-Market" },
                new Locations { CreatedOn = DateTime.Now, Name = "Walmart" }
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
                new Shopping { Description = "Groceries Shopping" },
                new Shopping { Description = "Clothes Shopping" }
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

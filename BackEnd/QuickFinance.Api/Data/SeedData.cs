using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;

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

        // Seed Categories
        var categories = new[]
        {
            new Category { Name = "Food", budgetlimit=300 },
            new Category { Name = "Transport", budgetlimit= 20.2M },
            new Category { Name = "Entertainment" },
            new Category { Name = "Utilities" },
            new Category { Name = "Health" }
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

        // Seed Budgets
        var budgets = new[]
        {
            new Budget { Month = "January", TotalBudget = 1000.00M },
            new Budget { Month = "February", TotalBudget = 800.00M }
        };

        context.Budgets.AddRange(budgets);
        context.SaveChanges(); // Save budgets

        // Seed Expenses - Now we can safely reference categories, budgets, and payment methods
        var expenses = new[]
        {
            new Expense { Description = "Groceries", Amount = 150.00M, BudgetId = budgets[0].Id, CategoryId = categories[0].Id, PaymentMethodId = paymentMethods[0].Id, DueDate = DateTime.Now, Executed= false },
            new Expense { Description = "Gas", Amount = 50.00M, BudgetId = budgets[0].Id, CategoryId = categories[1].Id, PaymentMethodId = paymentMethods[1].Id, DueDate = DateTime.Now, Executed = false },
            new Expense { Description = "Movie Tickets", Amount = 30.00M, BudgetId = budgets[1].Id, CategoryId = categories[2].Id, PaymentMethodId = paymentMethods[2].Id, DueDate = DateTime.Now, Executed=true }
        };

        context.Expenses.AddRange(expenses);
        context.SaveChanges(); // Save expenses
    }
}

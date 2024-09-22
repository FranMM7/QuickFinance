using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using QuickFinance.Api.Models;

namespace QuickFinance.Api.Data
{
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
                new Category { Name = "Food" },
                new Category { Name = "Transport" },
                new Category { Name = "Entertainment" },
                new Category { Name = "Utilities" },
                new Category { Name = "Health" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Payment Methods
            var paymentMethods = new[]
            {
                new PaymentMethod { Name = "Credit Card" },
                new PaymentMethod { Name = "Cash" },
                new PaymentMethod { Name = "Debit Card" },
                new PaymentMethod { Name = "Bank Transfer" }
            };

            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();

            // Seed Budgets
            var budgets = new[]
            {
                new Budget { Month = "January", TotalBudget = 1000.00M },
                new Budget { Month = "February", TotalBudget = 800.00M }
            };

            context.Budgets.AddRange(budgets);
            context.SaveChanges();

            // Seed Expenses
            var expenses = new[]
            {
                new Expense { Description = "Groceries", Amount = 150.00M, BudgetId = budgets[0].Id, CategoryId = categories[0].Id, PaymentMethodId = paymentMethods[0].Id, DueDate = DateTime.Now },
                new Expense { Description = "Gas", Amount = 50.00M, BudgetId = budgets[0].Id, CategoryId = categories[1].Id, PaymentMethodId = paymentMethods[1].Id, DueDate = DateTime.Now },
                new Expense { Description = "Movie Tickets", Amount = 30.00M, BudgetId = budgets[1].Id, CategoryId = categories[2].Id, PaymentMethodId = paymentMethods[2].Id, DueDate = DateTime.Now }
            };

            context.Expenses.AddRange(expenses);
            context.SaveChanges();

            // Seed Budget Limits
            var budgetLimits = new[]
            {
                new BudgetLimit { BudgetLimitAmount = 200.00M, CategoryId = categories[0].Id },
                new BudgetLimit { BudgetLimitAmount = 150.00M, CategoryId = categories[1].Id }
            };

            context.BudgetLimits.AddRange(budgetLimits);
            context.SaveChanges();
        }
    }
}

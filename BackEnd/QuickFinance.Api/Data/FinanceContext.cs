using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Models;

namespace QuickFinance.Api.Data
{
    public class FinanceContext : DbContext
    {
        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options) { }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Budget>()
                .Property(b => b.TotalBudget)
                .HasColumnType("money");

            modelBuilder.Entity<Category>()
                .Property(c => c.BudgetLimit)
                .HasColumnType("money");

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("money");
        }
    }
}

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
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        // For easy report view
        public DbSet<ExpensesSummaries> ExpensesSummaries { get; set; }
        public DbSet<BudgetSummary> BudgetSummaries { get; set; }
        public DbSet<CategorySummary> CategorySummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Exclude the Summary class from migrations
            modelBuilder.Ignore<BudgetSummary>();
            modelBuilder.Ignore<CategorySummary>();
            modelBuilder.Ignore<ExpensesSummaries>();

            // Automatically populate 'CreatedOn' with the current date when a new record is inserted
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetProperty("CreatedOn") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedOn")
                        .HasDefaultValueSql("GETDATE()"); // SQL Server default timestamp for creation
                }
            }

            // Enforce required fields and custom constraints

            // Budget entity
            modelBuilder.Entity<Budget>()
                .Property(b => b.Month)
                .IsRequired(); // Month is required

            modelBuilder.Entity<Budget>()
                .Property(b => b.TotalBudget)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); // TotalBudget is required, type is 'decimal(18,2)'

            // Category entity
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired(); // Name is required

            // We set the default value of budget limit to zero
            modelBuilder.Entity<Category>()
                .Property(b => b.budgetlimit)
                .HasDefaultValue(0);

            // Expense entity
            modelBuilder.Entity<Expense>()
                .Property(e => e.Description)
                .IsRequired(); // Description is required

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)"); // Specify precision for Amount field

            modelBuilder.Entity<Expense>()
                .Property(e => e.Executed)
                .HasDefaultValue(false); // By default, the value is false (indicating the expense is unpaid)

            // Expense's foreign key to Budget
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Budget)
                .WithMany(b => b.Expenses)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to prevent orphan records

            // PaymentMethod entity
            modelBuilder.Entity<PaymentMethod>()
                .Property(pm => pm.Name)
                .IsRequired(); // Name is required
        }
    }
}

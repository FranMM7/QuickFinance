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
        public DbSet<BudgetLimit> BudgetLimits { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatically populate 'createdon' with the current date when a new record is inserted
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetProperty("CreatedOn") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedOn")
                        .HasDefaultValueSql("GETDATE()"); // SQL Server default timestamp for creation
                }

                if (entityType.ClrType.GetProperty("UpdatedOn") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("UpdatedOn")
                        .ValueGeneratedOnUpdate()
                        .HasDefaultValueSql("GETDATE()"); // Automatically set 'updatedon' when modified
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

            // Expense entity
            modelBuilder.Entity<Expense>()
                .Property(e => e.Description)
                .IsRequired(); // Description is required

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)"); // Specify precision for Amount field

            modelBuilder.Entity<Expense>()
                .Property(e => e.Executed)
                .HasDefaultValue(false); //By default value as false indicated that this expense has not yet paid. 

            // Expense's foreign key to Budget
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Budget)
                .WithMany(b => b.Expenses)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to prevent orphan records

            // Category's relationship to Expenses
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Expenses) // A Category has many Expenses
                .WithOne(e => e.Category) // An Expense belongs to one Category
                .HasForeignKey(e => e.CategoryId) // Foreign key in Expense
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to prevent orphan records


            // BudgetLimits entity
            modelBuilder.Entity<BudgetLimit>()
                .Property(bl => bl.BudgetLimitAmount)
                .HasColumnType("decimal(18,2)"); // Specify precision for BudgetLimitAmount

            modelBuilder.Entity<BudgetLimit>()
                .HasOne(bl => bl.Category)
                .WithMany(c => c.BudgetLimits)
                .HasForeignKey(bl => bl.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete for category

            // PaymentMethod entity
            modelBuilder.Entity<PaymentMethod>()
                .Property(pm => pm.Name)
                .IsRequired(); // Name is required
        }

    }
}

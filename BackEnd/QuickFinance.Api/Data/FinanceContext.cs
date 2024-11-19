using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Models;

namespace QuickFinance.Api.Data
{
    public class FinanceContext : IdentityDbContext<ApplicationUser>
    {
        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options) { }

        #region DbSet Properties

        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<FinanceEvaluation> FinanceEvaluations { get; set; }
        public DbSet<FinanceDetail> FinanceDetails { get; set; }
        public DbSet<FinanceIncome> FinanceIncomes { get; set; }
        public DbSet<Shopping> Shoppings { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Settings> Settings { get; set; }

        // For easy report view
        public DbSet<DetailExpensesList> DetailExpensesList { get; set; }
        public DbSet<DetailBudgetList> DetailBudgetList { get; set; }
        public DbSet<DetailCategoryList> DetailCategoryList { get; set; }
        public DbSet<DetailShoppingList> DetailShoppingLists { get; set; }
        public DbSet<DetailFinanceList> DetailFinanceLists { get; set; }

        #endregion


        #region OnModelCreating Configuration

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Exclude the Details class from migrations
            modelBuilder.Ignore<DetailBudgetList>();
            modelBuilder.Ignore<DetailCategoryList>();
            modelBuilder.Ignore<DetailExpensesList>();
            modelBuilder.Ignore<DetailShoppingList>();
            modelBuilder.Ignore<DetailFinanceList>();

            // Automatically populate 'CreatedOn' with the current date when a new record is inserted
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetProperty("CreatedOn") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedOn")
                        .HasDefaultValueSql("GETDATE()");
                }

                if (entityType.ClrType.GetProperty("CreatedAt") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedAt")
                        .HasDefaultValueSql("GETDATE()");
                }

                // Set the status column to default 1 for all the entities that have such column
                if (entityType.ClrType.GetProperty("Status") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<int>("Status")
                        .HasDefaultValue(1);
                }

                if (entityType.ClrType.GetProperty("Amount") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");
                }

                if (entityType.ClrType.GetProperty("Subtotal") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<decimal>("Subtotal")
                        .HasColumnType("decimal(18,2)");
                }
            }

            #region userinfo    
            modelBuilder.Entity<ApplicationUser>()
                        .Property(u => u.AnonymousData)
                        .HasDefaultValue(true);
            #endregion

            #region Budget Configuration
            modelBuilder.Entity<Budget>()
                .Property(b => b.Title)
                .IsRequired();

            modelBuilder.Entity<Budget>()
                .Property(b => b.TotalAllocatedBudget)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);
            #endregion

            #region Category Configuration
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(b => b.BudgetLimit)
                .HasDefaultValue(0);

            modelBuilder.Entity<Category>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);
            #endregion

            #region Expense Configuration
            modelBuilder.Entity<Expense>()
                .Property(e => e.Description)
                .IsRequired();

            modelBuilder.Entity<Expense>()
                .Property(e => e.IsExecuted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Budget)
                .WithMany(b => b.Expenses)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region PaymentMethod Configuration
            modelBuilder.Entity<PaymentMethod>()
                .Property(pm => pm.PaymentMethodName)
                .IsRequired();
            #endregion

            #region FinanceEvaluation Configuration
            modelBuilder.Entity<FinanceEvaluation>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);
            #endregion

            #region FinanceDetail Configuration
            modelBuilder.Entity<FinanceDetail>()
                .HasOne(fd => fd.FinanceEvaluation)
                .WithMany(fe => fe.FinanceDetails)
                .HasForeignKey(fd => fd.FinanceEvaluationId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<FinanceDetail>()
                .HasOne(fd => fd.Category)
                .WithMany()
                .HasForeignKey(fd => fd.CategoryId) // Use CategoryId for the relationship
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region FinanceIncome Configuration
            modelBuilder.Entity<FinanceIncome>()
               .HasOne(fi => fi.FinanceEvaluation)
               .WithMany(fe => fe.FinancesIncomes)
               .HasForeignKey(fi => fi.FinanceId)
               .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Shopping Configuration
            modelBuilder.Entity<Shopping>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);
            #endregion

            #region ShoppingList Configuration
            modelBuilder.Entity<ShoppingList>()
                .HasOne(sl => sl.Shopping)
                .WithMany(s => s.ShoppingLists)
                .HasForeignKey(sl => sl.ShoppingId);

            modelBuilder.Entity<ShoppingList>()
                .HasOne(sl => sl.Category)
                .WithMany()
                .HasForeignKey(sl => sl.CategoryId);

            modelBuilder.Entity<ShoppingList>()
                .HasOne(sl => sl.Locations)
                .WithMany()
                .HasForeignKey(sl => sl.LocationId);

            modelBuilder.Entity<ShoppingList>()
                .Property<int>("Quantity")
                .HasDefaultValue(1);

            modelBuilder.Entity<ShoppingList>()
               .Property(f => f.Subtotal)
               .HasComputedColumnSql("[Quantity] * [Amount]");
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}

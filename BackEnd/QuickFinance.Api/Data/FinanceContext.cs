using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Models;

namespace QuickFinance.Api.Data
{
    public class FinanceContext : DbContext
    {
        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; } //global categories for modules budgets, finance or shopping 
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<FinanceEvaluation> FinanceEvaluations { get; set; } // to check the type of the monthly expense they do, important, ant, ghost, vampire expenses
        public DbSet<FinanceDetail> FinanceDetails { get; set; }
        public DbSet<Shopping> Shoppings { get; set; } //to allow quick shopping on their favorite supermarket or other stores
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; } //to see how frencuently they pay with cc, db, etc. 

        // For easy report view
        public DbSet<DetailExpensesList> detailExpensesList { get; set; }
        public DbSet<DetailBudgetList> detailBudgetList { get; set; }
        public DbSet<DetailCategoryList> detailCategoryList { get; set; }
        public DbSet<DetailShoppingList> detailShoppingLists { get; set; }
        public DbSet<DetailFinanceList> detailFinanceLists { get; set; }
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
                        .HasDefaultValueSql("GETDATE()"); // SQL Server default timestamp for creation
                }

                if (entityType.ClrType.GetProperty("CreatedAt") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedAt")
                        .HasDefaultValueSql("GETDATE()"); // SQL Server default timestamp for creation
                }

                //set the status column to default 1 for all the entities that has such column 
                if (entityType.ClrType.GetProperty("Status") != null)
                {
                    modelBuilder.Entity(entityType.Name).Property<int>("Status")
                        .HasDefaultValue(1);
                }
            }


            // Enforce required fields and custom constraints

            // Budget entity
            modelBuilder.Entity<Budget>()
                .Property(b => b.Title)
                .IsRequired(); // Month is required

            modelBuilder.Entity<Budget>()
                .Property(b => b.TotalAllocatedBudget)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); // TotalBudget is required, type is 'decimal(18,2)'

            // Category entity
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired(); // Name is required

            // We set the default value of budget limit to zero
            modelBuilder.Entity<Category>()
                .Property(b => b.BudgetLimit)
                .HasDefaultValue(0);

            // Expense entity
            modelBuilder.Entity<Expense>()
                .Property(e => e.Description)
                .IsRequired(); // Description is required

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)"); // Specify precision for Amount field

            modelBuilder.Entity<Expense>()
                .Property(e => e.IsExecuted)
                .HasDefaultValue(false); // By default, the value is false (indicating the expense is unpaid)

            // Expense's foreign key to Budget
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Budget)
                .WithMany(b => b.Expenses)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to prevent orphan records

            // PaymentMethod entity
            modelBuilder.Entity<PaymentMethod>()
                .Property(pm => pm.PaymentMethodName)
                .IsRequired(); // Name is required


            // Finance Entity
            modelBuilder.Entity<FinanceDetail>()
            .HasOne(fd => fd.FinanceEvaluation)
            .WithMany(fe => fe.FinanceDetails)
            .HasForeignKey(fd => fd.FinanceId);

            modelBuilder.Entity<FinanceDetail>()
                .HasOne(fd => fd.Category)
                .WithMany()
                .HasForeignKey(fd => fd.CategoryId);


            //Shopping Entity. 
           
            modelBuilder.Entity<ShoppingList>()
                .HasOne(sl => sl.Shopping)
                .WithMany(s => s.ShoppingLists)
                .HasForeignKey(sl => sl.ShoppingId);

            modelBuilder.Entity<ShoppingList>()
                .HasOne(sl => sl.Category)
                .WithMany()
                .HasForeignKey(sl => sl.CategoryId);

            modelBuilder.Entity<ShoppingList>()
                .HasOne(sl=> sl.Locations )
                .WithMany()
                .HasForeignKey(sl => sl.LocationId);

            modelBuilder.Entity<ShoppingList>()
                .Property<int>("Quantity")
                .HasDefaultValue(1);

            modelBuilder.Entity<ShoppingList>()
               .Property(f => f.Subtotal)
               .HasComputedColumnSql("[Quantity] * [Amount]"); // SQL computation for the field


            base.OnModelCreating(modelBuilder);
        }
    }
}

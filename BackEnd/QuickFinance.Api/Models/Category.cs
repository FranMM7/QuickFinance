namespace QuickFinance.Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal BudgetLimit { get; set; } // Monthly budget for the category
    }
}

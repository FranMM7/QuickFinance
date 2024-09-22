namespace QuickFinance.Api.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public decimal TotalBudget { get; set; }
        public DateTime Month { get; set; } // Tracks the budget by month
    }
}

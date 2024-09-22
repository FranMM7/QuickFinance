namespace QuickFinance.Api.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; } // Cash, Credit Card, etc.
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

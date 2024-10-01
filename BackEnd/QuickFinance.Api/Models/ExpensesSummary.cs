using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;

public class ExpensesSummaries
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public int CategoryId { get; set; }
    public string Category { get; set; }
    public int BudgetId { get; set; }
    public string Month { get; set; }
    public decimal TotalBudget { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentMethod { get; set; }
    public bool Executed { get; set; }
    public DateTime ModifiedOn { get; set; }
}
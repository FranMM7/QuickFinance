using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;

public class BudgetSummary
{
    public int Id { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal ExecutedBudget { get; set; }
    public string Month { get; set; }
    public DateTime ModifiedOn { get; set; }

}
using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;

public class DetailBudgetList
{
    public int Id { get; set; }
    public decimal TotalAllocatedBudget { get; set; }
    public decimal ExecutedBudget { get; set; }
    public string Title { get; set; }
    public DateTime ModifiedOn { get; set; }

}
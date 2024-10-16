using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
public class DetailCategoryList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal BudgetLimit { get; set; }
    public int ExpenseCount { get; set; }
    public decimal TotalExpended { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; } // Can be null
    public DateTime ModifiedOn { get; set; }
}

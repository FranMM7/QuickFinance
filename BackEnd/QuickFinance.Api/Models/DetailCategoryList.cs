using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
public class DetailCategoryList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal BudgetLimit { get; set; }
    public int InUse { get; set; }
    public decimal BudgetTotalExpended { get; set; }
    public decimal BudgetTotalExpendedExecuted { get; set; }
    public decimal ShoppingTotalExpended { get; set; }
    public DateTime ModifiedOn { get; set; }
}

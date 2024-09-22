using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;

public class Category
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    [Required] // Optional, if you prefer data annotations
    public string Name { get; set; }

    public virtual ICollection<BudgetLimit> BudgetLimits { get; set; } // Virtual for lazy loading
    public virtual ICollection<Expense> Expenses { get; set; } // Virtual for lazy loading
}

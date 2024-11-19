
using System.ComponentModel.DataAnnotations;

public class BudgetDto
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } // Required

    [Required]
    public decimal TotalAllocatedBudget { get; set; } // Required
    public int State { get; set; } = 1; // 1=active, 0=inactive
    public string userId { get; set; }

    public List<ExpenseDto> ExpensesDTO { get; set; } // List of associated expenses
}
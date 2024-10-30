using System.ComponentModel.DataAnnotations;

public class ExpenseDto
{
    public int Id { get; set; }

    [Required]
    public int BudgetId { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public DateTime? ExpenseDueDate { get; set; }

    public int? PaymentMethodId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    public bool IsExecuted { get; set; }
}
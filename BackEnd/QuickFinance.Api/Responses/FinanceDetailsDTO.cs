using System.ComponentModel.DataAnnotations;
public class FinanceDetailsDTO
{
    public string Description { get; set; }
    public int ExpenseCategory { get; set; } // 1 = Important, 2 = Ghost, 3 = Ant, 4 = Vampire
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
}
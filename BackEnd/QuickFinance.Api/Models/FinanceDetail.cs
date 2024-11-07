using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class FinanceDetail
{
    public int Id { get; set; }
    public int FinanceId { get; set; }
    public string Description { get; set; }
    public int ExpenseCategory { get; set; } // 1 = Important, 2 = Ghost, 3 = Ant, 4 = Vampire
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }

    [JsonIgnore]
    public FinanceEvaluation FinanceEvaluation { get; set; }

    [JsonIgnore]
    public Category Category { get; set; }
}
using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
public class FinanceDetail
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int ExpenseCategory { get; set; } // 1 = Important, 2 = Ghost, 3 = Ant, 4 = Vampire
    public decimal Amount { get; set; }

    [ForeignKey("FinanceEvaluation")]
    public int FinanceEvaluationId { get; set; }

    [JsonIgnore]
    public virtual FinanceEvaluation FinanceEvaluation { get; set; }
    public int CategoryId { get; set; }
    [JsonIgnore]
    public virtual Category Category { get; set; }
}

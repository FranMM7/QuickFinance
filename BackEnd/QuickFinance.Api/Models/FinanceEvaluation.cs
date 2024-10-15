using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class FinanceEvaluation
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string Title { get; set; }
    public int State { get; set; } = 1; //1=active, 0=inactive
    public ICollection<FinanceDetail> FinanceDetails { get; set; }
}
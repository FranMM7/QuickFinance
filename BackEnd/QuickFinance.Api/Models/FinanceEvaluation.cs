using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class FinanceEvaluation
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now; // Auto-populated by the database
    public DateTime? UpdatedOn { get; set; }
    public string Title { get; set; } = ""; // Required
    public decimal TotalIncomes { get; set; }
    public decimal TotalExpenses { get; set; }
    public int State { get; set; } = 1; //1=active, 0=inactive

    public string UserId { get; set; }
    [JsonIgnore]
    public virtual ApplicationUser User { get; set; }  // Navigation property
                                                     
    public ICollection<FinanceDetail> FinanceDetails { get; set; }
    public ICollection<FinanceIncome> FinancesIncomes { get; set; }

}
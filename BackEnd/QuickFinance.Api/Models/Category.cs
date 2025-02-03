using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Category
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now; // Auto-populated by the database
    public DateTime? UpdatedOn { get; set; } //can be null
    public string Name { get; set; } = "";  
    public decimal BudgetLimit { get; set; } = 0m; //default value
    public bool TypeBudget { get; set; } = true;
    public bool TypeShoppingList { get; set; } = false;
    public bool TypeFinanceAnalizis { get; set; } = false;
    public int State { get; set; } = 1; //1=active, 0=inactive

    public string UserId { get; set; }
    [JsonIgnore]
    public virtual ApplicationUser User { get; set; }  // Navigation property


}

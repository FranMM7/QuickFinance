using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Category
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; } //can be null
    public string Name { get; set; }   
    public decimal budgetlimit { get; set; } = 0m; //default value
    public bool TypeBudget { get; set; }
    public bool TypeShoppingList { get; set; }
    public bool TypeFinanceAnalizis { get; set; }

}

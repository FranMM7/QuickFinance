using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Category
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; } //can be null
    public string Name { get; set; }

    public DateTime ModifiedOn { get; set; }

    public decimal budgetlimit { get; set; } = 0m; //default value

    [JsonIgnore] // This will ignore the property during serialization and deserialization
    public int ExpenseCount { get; set; }

    [JsonIgnore]
    public decimal TotalExpended { get; set; }
}

using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Shopping
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string Description { get; set; }
    public ICollection<ShoppingList> ShoppingLists { get; set; }
}
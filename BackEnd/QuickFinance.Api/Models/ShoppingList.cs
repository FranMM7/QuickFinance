using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class ShoppingList
{
    public int Id { get; set; }
    public int ShoppingId { get; set; }
    public int? CategoryId { get; set; }
    public int? LocationId { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Amount { get; set; } = decimal.Zero;
    public decimal Subtotal { get; private set; }
    public Shopping Shopping { get; set; }
    public Category Category { get; set; }
    public Locations Locations { get; set; }
}
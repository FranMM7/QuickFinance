using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
public class ShoppingList
{
    [Column("Id")]
    public int Id { get; set; }
    [Column("ShoppingId")]
    public int ShoppingId { get; set; }
    [Column("CategoryId")]
    public int? CategoryId { get; set; }
    [Column("LocationId")]
    public int? LocationId { get; set; }
    [Column("ItemName")]
    public string ItemName { get; set; }
    [Column("Brand")]
    public string? Brand { get; set; }
    [Column("Quantity")]
    public int Quantity { get; set; } = 1;
    [Column("Amount")]
    public decimal Amount { get; set; } = decimal.Zero;
    [Column("Subtotal")]
    public decimal Subtotal { get; private set; }
    public Shopping Shopping { get; set; }
    public Category Category { get; set; }
    public Locations Locations { get; set; }
}
public class ShoppingItemsDTO
{
    public int? CategoryId { get; set; }
    public int? LocationId { get; set; }
    public string ItemName { get; set; }
    public string? Brand { get; set; }
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
}
public class PriceIncreaseByBrandAddProductDTO
{
    public string Brand { get; set; }
    public string ItemName { get; set; }
    public decimal TotalByItem { get; set; }
    public decimal LowestPrice { get; set; }
    public decimal HighestPrice { get; set; }
    public decimal IncreasePercentage { get; set; }
    public string UserId { get; set; }
}

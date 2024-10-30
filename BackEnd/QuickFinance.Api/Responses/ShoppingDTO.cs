public class ShoppingDTO
{
    public int Id { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string Description { get; set; }
    public int State { get; set; } = 1; //1=active, 0=inactive
    public decimal GrandTotal { get; set; }
}
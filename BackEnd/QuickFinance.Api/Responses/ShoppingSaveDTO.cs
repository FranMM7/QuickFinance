public class ShoppingSaveDTO
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string userId { get; set; }
    public ICollection<ShoppingItemsDTO> ShoppingLists { get; set; }
}
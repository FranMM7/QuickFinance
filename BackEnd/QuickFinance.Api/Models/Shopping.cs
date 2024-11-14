using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Shopping
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string Description { get; set; }
    public int State { get; set; } = 1; //1=active, 0=inactive
    public string UserId { get; set; }

    [JsonIgnore]
    public virtual ApplicationUser User { get; set; }  // Navigation property
    public ICollection<ShoppingList> ShoppingLists { get; set; }

}
using System.ComponentModel;

public class CategoryDTO
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; } //can be null
    public string Name { get; set; }
    public decimal BudgetLimit { get; set; } = 0m; //default value
    public bool TypeBudget { get; set; } = true;
    public bool TypeShoppingList { get; set; } = false;
    public bool TypeFinanceAnalizis { get; set; } = false;

    [DefaultValue(1)]
    public int State { get; set; } //1=active, 0=inactive
    public string UserId { get; set; }
}
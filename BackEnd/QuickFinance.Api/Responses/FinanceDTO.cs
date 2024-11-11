using QuickFinance.Api.Models;
using System.ComponentModel.DataAnnotations;
public class FinanceDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ICollection<FinanceDetailsDTO> FinanceDetails { get; set; }
    public ICollection<FinanceIncomeDTO> FinanceIncomes { get; set; }
}
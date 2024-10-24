using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickFinance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GeneralController : ControllerBase
    {
        private readonly FinanceContext _context;

        public GeneralController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("TotalPages")]
        public async Task<ActionResult<int>> getPaginationInfo(int RowsPage, string tableName, int Id=0)
        {
            int totalRecords = 0;

            switch (tableName.ToLower()) // Ensure case-insensitivity
            {
                case "category":
                    totalRecords = await _context.Categories.CountAsync(b => b.State == 1);
                    break;
                case "budgets":
                    totalRecords = await _context.Budgets.CountAsync(b => b.State == 1);
                    break;
                case "Locations":
                    totalRecords = await _context.Locations.CountAsync(b => b.State == 1);
                    break;
                case "expenses":
                    if (Id == 0)
                        return BadRequest("Id is required");
                    totalRecords = await _context.Expenses.CountAsync(b => b.BudgetId == Id);
                    break;
                case "shoppinglist":
                    if (Id == 0)
                        return BadRequest("Id is required");
                    totalRecords = await _context.ShoppingLists.CountAsync(b => b.ShoppingId == Id);
                    break;
                case "financedetail":
                    totalRecords = await _context.FinanceDetails.CountAsync(b => b.FinanceId == Id);
                    break;
                // Add more cases for other tables as needed
                default:
                    return BadRequest("Invalid table name.");
            }

            // Calculate total pages
            int totalPages = (int)Math.Ceiling((double)totalRecords / RowsPage);

            return totalPages;
        }


    }
}
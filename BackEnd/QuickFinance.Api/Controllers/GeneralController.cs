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

        //[HttpGet("TotalPages")]
        //public async Task<ActionResult<int>> getPaginationInfo(int RowsPage, string tableName, int Id = 0)
        //{
        //    int totalRecords = 0;

        //    switch (tableName.ToLower()) // Ensure case-insensitivity
        //    {
        //        case "category":
        //            totalRecords = await _context.Categories.CountAsync(b => b.State == 1);
        //            break;
        //        case "budgets":
        //            totalRecords = await _context.Budgets.CountAsync(b => b.State == 1);
        //            break;
        //        case "Locations":
        //            totalRecords = await _context.Locations.CountAsync(b => b.State == 1);
        //            break;
        //        case "expenses":
        //            if (Id == 0)
        //                return BadRequest("Id is required");
        //            totalRecords = await _context.Expenses.CountAsync(b => b.BudgetId == Id);
        //            break;
        //        case "shoppinglist":
        //            if (Id == 0)
        //                return BadRequest("Id is required");
        //            totalRecords = await _context.ShoppingLists.CountAsync(b => b.ShoppingId == Id);
        //            break;
        //        case "financedetail":
        //            totalRecords = await _context.FinanceDetails.CountAsync(b => b.FinanceEvaluationId == Id);
        //            break;
        //        // Add more cases for other tables as needed
        //        default:
        //            return BadRequest("Invalid table name.");
        //    }

        //    // Calculate total pages
        //    int totalPages = (int)Math.Ceiling((double)totalRecords / RowsPage);

        //    return totalPages;
        //}


        [HttpPost("saveSettings")]
        public async Task<ActionResult<int>> SaveSettings(string userId, [FromBody] Settings settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserID is required");
                }

                if (settings == null)
                {
                    return BadRequest("Settings data is required");
                }

                settings.UserId = userId; // Ensure the setting has the correct UserId

                _context.Settings.Add(settings);

                int result = await _context.SaveChangesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getSettings")]
        public async Task<ActionResult<Settings>> GetSettings(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserID is required");
                }

                var setting = await _context.Settings.FirstOrDefaultAsync(u => u.UserId == userId);

                if (setting == null)
                {
                    return NotFound($"No settings found for user with ID '{userId}'");
                }

                return Ok(setting);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
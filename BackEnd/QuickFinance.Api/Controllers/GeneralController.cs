using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using System.Collections.Generic;
using System.Text.Json;
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

        [Authorize]
        [HttpPost("saveSettings")]
        public async Task<ActionResult<int>> SaveSettings([FromBody] SettingsDTO settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (settings == null)
                {
                    return BadRequest("Settings data is required");
                }

                // Fetch the user from the database based on userId
                var user = await _context.Users.FindAsync(settings.UserId);
                if (user == null)
                {
                    return BadRequest("User not found");
                }

                var newRecord = new Settings
                {
                    Id = 0,
                    UserId = user.Id,
                    JsonValue = settings.JsonValue,
                    SettingsName = settings.SettingsName
                };

                // Add settings to the context
                _context.Settings.Add(newRecord);

                // Save the changes to the database
                int result = await _context.SaveChangesAsync();

                // Deserialize the JsonValue to extract the Language
                var settingsJson = JsonSerializer.Deserialize<Dictionary<string, string>>(settings.JsonValue);
                var language = settingsJson != null && settingsJson.ContainsKey("Language") ? settingsJson["Language"] : string.Empty;

                // Use the extracted Language in the stored procedure
                var sql = "EXEC [dbo].[stp_AddUserCategoriesAndLocations] @userID, @lang";
                await _context.Database.GetDbConnection().QueryAsync(
                    sql,
                    new { userID = settings.UserId, lang = language } // Pass the language value
                );

                // Return the result
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("updateSettings")]
        public async Task<ActionResult<int>> updateSettings([FromBody] SettingsDTO[] settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (settings == null)
                {
                    return BadRequest("Settings data is required");
                }

                // Loop through the settings array
                foreach (var setting in settings)
                {
                    // Fetch the user from the database based on userId
                    var user = await _context.Users.FindAsync(setting.UserId);
                    if (user == null)
                    {
                        return BadRequest($"User with ID {setting.UserId} not found");
                    }

                    // Fetch the existing settings for the user
                    var existingSettings = await _context.Settings
                        .Where(s => s.UserId == user.Id && s.SettingsName == setting.SettingsName)
                        .ToListAsync();

                    if (existingSettings == null || !existingSettings.Any())
                    {
                        return BadRequest($"No existing settings found for the user {setting.UserId} with SettingsName {setting.SettingsName}");
                    }

                    // Loop through each existing setting and replace its JsonValue
                    foreach (var existingSetting in existingSettings)
                    {
                        // Directly replace the JsonValue with the new one
                        existingSetting.JsonValue = setting.JsonValue;
                    }
                }

                // Save the changes to the database
                int result = await _context.SaveChangesAsync();

                // Return the result
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
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

        [Authorize]
        [HttpGet("getPriceIncreaseByProduct")]
        public async Task<IActionResult> GetPriceIncreaseByProduct(string userId)
        {
            // Ensure the userId is not null or empty
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is required.");

            // Execute the raw SQL query on the view
            var query = await _context.VwPriceIncreaseByProduct
                .FromSqlRaw(@"
            SELECT TOP (1000) 
                [ItemName],
                [TotalByItem],
                [LowestPrice],
                [HighestPrice],
                [IncreasePercentage],
                [UserId]
            FROM [vw_PriceIncreaseByProduct]
            WHERE UserId = {0}", userId)
                .ToListAsync();

            if (!query.Any())
                return NotFound("No price increase data found for the specified user.");

            return Ok(query);
        }

        [Authorize]
        [HttpGet("getPriceIncreaseByCategoryAndProduct")]
        public async Task<IActionResult> GetPriceIncreaseByCategoryAndProduct(string userId)
        {
            // Ensure the userId is not null or empty
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is required.");

            // Execute the raw SQL query on the view
            var query = await _context.VwPriceIncreaseByCategoryAndProduct
                .FromSqlRaw(@"
            SELECT TOP (1000) 
                [Category],
                [ItemName],
                [TotalByItem],
                [LowestPrice],
                [HighestPrice],
                [IncreasePercentage],
                [UserId]
            FROM [dbo].[vw_PriceIncreaseByCategoryAndProduct]
            WHERE UserId = {0}", userId)
                .ToListAsync();

            if (!query.Any())
                return NotFound("No price increase data found for the specified user.");

            return Ok(query);
        }


        [Authorize]
        [HttpGet("getPriceIncreaseByBrandAndProduct")]
        public async Task<IActionResult> GetPriceIncreaseByBrandAndProduct(string userId)
        {
            // Ensure the userId is not null or empty
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is required.");

            // Execute the raw SQL query on the view
            var query = await _context.VwPriceIncreaseByBrandAndProduct
                .FromSqlRaw(@"
            SELECT TOP (1000) 
                [Brand],
                [ItemName],
                [TotalByItem],
                [LowestPrice],
                [HighestPrice],
                [IncreasePercentage],
                [UserId]
            FROM [dbo].[vw_PriceIncreaseByBrandAndProduct]
            WHERE UserId = {0}", userId)
                .ToListAsync();

            if (!query.Any())
                return NotFound("No price increase data found for the specified user.");

            return Ok(query);
        }





    }
}
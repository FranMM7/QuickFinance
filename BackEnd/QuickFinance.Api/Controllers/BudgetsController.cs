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
    public class BudgetsController : ControllerBase
    {
        private readonly FinanceContext _context;

        public BudgetsController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("BudgetsInfo")]
        public async Task<ActionResult<string>> GetBudgetInfo()
        {
            var sql = "EXEC [DBO].[sp_GetBudgetOverviewJSON]";

            // Use Dapper's ExecuteScalarAsync to execute the stored procedure and return the JSON
            var result = await _context.Database.GetDbConnection().ExecuteScalarAsync<string>(sql);

            // Return the result as an Ok response
            return Ok(result);
        }

        [HttpGet("Summary")]
        public async Task<ActionResult<IEnumerable<BudgetSummary>>> GetBudgetSummary(int PageNumber)
        {
            var sql = "EXEC [dbo].[GetBudgetDetails] @PageNumber";

            // Using Dapper for more efficient data retrieval
            var budgetSummaries = await _context.Database.GetDbConnection().QueryAsync<BudgetSummary>(sql, new {PageNumber= PageNumber });

            return Ok(budgetSummaries);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Budget>> GetBudget(int id)
        {
            var budget = await _context.Budgets.Include(b => b.Expenses)
                                                 .FirstOrDefaultAsync(b => b.Id == id);

            if (budget == null)
            {
                return NotFound();
            }

            return budget;
        }

        [HttpPost]
        public async Task<ActionResult<Budget>> PostBudget(Budget budget)
        {
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudget", new { id = budget.Id }, budget);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudget(int id, Budget budget)
        {
            if (id != budget.Id)
            {
                return BadRequest();
            }

            _context.Entry(budget).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool BudgetExists(int id)
        {
            return _context.Budgets.Any(b => b.Id == id);
        }
    }
}

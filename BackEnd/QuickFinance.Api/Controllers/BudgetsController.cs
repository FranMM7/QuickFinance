using Microsoft.AspNetCore.Mvc;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using Microsoft.EntityFrameworkCore;

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

        // GET: api/budgets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgets()
        {
            return await _context.Budgets.ToListAsync(); // Retrieve all budgets from the database
        }

        // POST: api/budgets
        [HttpPost]
        public async Task<ActionResult<Budget>> CreateBudget(Budget budget)
        {
            _context.Budgets.Add(budget); // Add the new budget to the context
            await _context.SaveChangesAsync(); // Save changes to the database
            return CreatedAtAction(nameof(GetBudgets), new { id = budget.Id }, budget); // Return the created budget
        }

        // PUT: api/budgets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(int id, Budget budget)
        {
            if (id != budget.Id) // Check if the provided ID matches the budget ID
                return BadRequest();

            _context.Entry(budget).State = EntityState.Modified; // Mark the budget as modified
            await _context.SaveChangesAsync(); // Save changes to the database
            return NoContent(); // Return a 204 No Content response
        }

        // DELETE: api/budgets/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _context.Budgets.FindAsync(id); // Find the budget by ID
            if (budget == null)
                return NotFound(); // Return 404 if the budget is not found

            _context.Budgets.Remove(budget); // Remove the budget from the context
            await _context.SaveChangesAsync(); // Save changes to the database
            return NoContent(); // Return a 204 No Content response
        }
    }
}

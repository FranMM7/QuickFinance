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
    public class BudgetLimitsController : ControllerBase
    {
        private readonly FinanceContext _context;

        public BudgetLimitsController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetLimit>>> GetBudgetLimits()
        {
            return await _context.BudgetLimits.Include(bl => bl.Category).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetLimit>> GetBudgetLimit(int id)
        {
            var budgetLimit = await _context.BudgetLimits.Include(bl => bl.Category)
                                                         .FirstOrDefaultAsync(bl => bl.Id == id);

            if (budgetLimit == null)
            {
                return NotFound();
            }

            return budgetLimit;
        }

        [HttpPost]
        public async Task<ActionResult<BudgetLimit>> PostBudgetLimit(BudgetLimit budgetLimit)
        {
            _context.BudgetLimits.Add(budgetLimit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudgetLimit", new { id = budgetLimit.Id }, budgetLimit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudgetLimit(int id, BudgetLimit budgetLimit)
        {
            if (id != budgetLimit.Id)
            {
                return BadRequest();
            }

            _context.Entry(budgetLimit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetLimitExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudgetLimit(int id)
        {
            var budgetLimit = await _context.BudgetLimits.FindAsync(id);
            if (budgetLimit == null)
            {
                return NotFound();
            }

            _context.BudgetLimits.Remove(budgetLimit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetLimitExists(int id)
        {
            return _context.BudgetLimits.Any(bl => bl.Id == id);
        }
    }
}

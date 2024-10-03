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
    public class CategoriesController : ControllerBase
    {
        private readonly FinanceContext _context;

        public CategoriesController(FinanceContext context)
        {
            _context = context;
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            // Add the new category to the context
            _context.Categories.Add(category);

            // Save changes asynchronously
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        [HttpGet("Summary")]
        public async Task<ActionResult<IEnumerable<CategorySummary>>> GetCategorySummary()
        {
            // Call the stored procedure and map it to CategorySummary
            var categories = await _context.CategorySummaries
                .FromSqlRaw("EXECUTE dbo.[GetCategoryDetails]")
                .ToListAsync();

            return Ok(categories);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                _context.Entry(category).Entity.UpdatedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }
    }
}

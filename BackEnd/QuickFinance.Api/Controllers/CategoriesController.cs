using Microsoft.AspNetCore.Mvc;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using Microsoft.EntityFrameworkCore;

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

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync(); // Retrieve all categories from the database
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            _context.Categories.Add(category); // Add the new category to the context
            await _context.SaveChangesAsync(); // Save changes to the database
            return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category); // Return the created category
        }

        // PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.Id) // Check if the provided ID matches the category ID
                return BadRequest();

            _context.Entry(category).State = EntityState.Modified; // Mark the category as modified
            await _context.SaveChangesAsync(); // Save changes to the database
            return NoContent(); // Return a 204 No Content response
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id); // Find the category by ID
            if (category == null)
                return NotFound(); // Return 404 if the category is not found

            _context.Categories.Remove(category); // Remove the category from the context
            await _context.SaveChangesAsync(); // Save changes to the database
            return NoContent(); // Return a 204 No Content response
        }
    }
}

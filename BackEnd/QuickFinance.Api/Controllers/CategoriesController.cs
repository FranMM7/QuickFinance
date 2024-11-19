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
    public class CategoriesController : ControllerBase
    {
        private readonly FinanceContext _context;

        public CategoriesController(FinanceContext context)
        {
            _context = context;
        }


        //add category
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

        //api/Categories
        [HttpGet("CategoriesType")]
        public async Task<ActionResult<IEnumerable<Category>>> getCategoryList(int type)
        {
            // Initialize the list as an empty enumerable
            IEnumerable<Category> list = new List<Category>();

            switch (type)
            {
                case 1:
                    list = await _context.Categories
                        .Where(b => b.TypeBudget == true) // Apply the filter here
                        .ToListAsync(); // Use ToListAsync for async operation
                    break;
                case 2:
                    list = await _context.Categories
                        .Where(b => b.TypeFinanceAnalizis == true) // Apply the filter here
                        .ToListAsync(); // Use ToListAsync for async operation
                    break;
                case 3:
                    list = await _context.Categories
                        .Where(b => b.TypeShoppingList == true) // Apply the filter here
                        .ToListAsync(); // Use ToListAsync for async operation
                    break;
                // Add additional cases for different types as needed
                default:
                    return BadRequest("Invalid type parameter.");
            }

            return Ok(list); // Return the filtered list
        }


        //api/Categories/Summary
        [HttpGet("List")]
        public async Task<ActionResult<PagedResponse<IEnumerable<DetailCategoryList>>>> GetCategoryList(string userId, int pageNumbers = 1, int rowsPerPage = 10)
        {
            // Construct the SQL query string to execute the stored procedure
            var sql = "EXECUTE dbo.[stp_GetBudgetDetails] @userId, @PageNumber, @RowsPage";

            // Execute the stored procedure with the parameter with dapper
            var categories = await _context.Database.GetDbConnection().QueryAsync<DetailCategoryList>(sql, new { userId= userId, PageNumber = pageNumbers, RowsPage=rowsPerPage});

            // Count total records in the database that are active (State == 1)
            var totalRecords = await _context.Categories
                .Where(r => r.UserId == userId && r.State == 1)
                .CountAsync();


            // Create the pagination response
            var pagedResponse = new PagedResponse<IEnumerable<DetailCategoryList>>(
                categories, // The list of Categories details
                pageNumbers, // Current page number
                rowsPerPage, // Number of rows per page
                totalRecords // Total records in the database
            );


            // Construct URIs for pagination metadata
            var baseUri = $"{Request.Scheme}://{Request.Host}/api/Categories/List";
            pagedResponse.FirstPage = new Uri($"{baseUri}?pageNumber=1&rowsPerPage={rowsPerPage}");
            pagedResponse.LastPage = new Uri($"{baseUri}?pageNumber={pagedResponse.TotalPages}&rowsPerPage={rowsPerPage}");
            pagedResponse.NextPage = pageNumbers < pagedResponse.TotalPages
                ? new Uri($"{baseUri}?pageNumber={pageNumbers + 1}&rowsPerPage={rowsPerPage}")
                : null;
            pagedResponse.PreviousPage = pageNumbers > 1
                ? new Uri($"{baseUri}?pageNumber={pageNumbers - 1}&rowsPerPage={rowsPerPage}")
                : null;

            // Return the paged response with the budget details
            return Ok(pagedResponse);
        }


        //api/Categories/{id}
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

        //update category
        //api/Categories/{id}
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

            return Ok();
        }

        // API route to change the state of a record 
        [HttpPut("ChangeState")]
        public async Task<IActionResult> ChangeStateCategory(int id)
        {
            try
            {
                // Find the shopping record by ID
                var record = await _context.Categories.FirstOrDefaultAsync(b => b.Id == id);

                if (record == null)
                {
                    return NotFound();
                }

                // Disable the shopping record by setting its State to 0 (inactive)
                record.State = record.State == 1 ? 0 : 1;
                record.UpdatedOn = DateTime.UtcNow;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //delete category
        //api/Categories/{id}
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

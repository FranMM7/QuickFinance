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
    public class ShoppingController : ControllerBase
    {
        private readonly FinanceContext _context;

        public ShoppingController(FinanceContext context)
        {
            _context = context;
        }

        //api/Shopping
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingDTO>>> GetShopping(int pageNumber = 1, int pageSize = 10)
        {
            // Calculate total count of distinct shopping records
            var totalCount = await _context.Shoppings.CountAsync();

            // Retrieve shopping records with pagination and grouping
            var shoppingData = await _context.Shoppings
                .GroupJoin(
                    _context.ShoppingLists,
                    shopping => shopping.Id,
                    shoppingList => shoppingList.ShoppingId,
                    (shopping, shoppingLists) => new
                    {
                        Shopping = shopping,
                        GrandTotal = shoppingLists.Sum(s => s.Subtotal)
                    })
                .Select(s => new ShoppingDTO
                {
                    Id = s.Shopping.Id,
                    ModifiedOn = s.Shopping.UpdatedOn ?? s.Shopping.CreatedOn,
                    Description = s.Shopping.Description,
                    State = s.Shopping.State,
                    GrandTotal = s.GrandTotal
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber,
                ShoppingLists = shoppingData
            };

            return Ok(result);
        }

        //api/Shopping/List
        [HttpGet("List")]
        public async Task<ActionResult<PagedResponse<IEnumerable<DetailShoppingList>>>> GetShoppingList(int Id, int pageNumber = 0, int rowsPerPage = 0)
        {
            // Count total records in the database
            var totalRecords = await _context.ShoppingLists.CountAsync(b => b.ShoppingId == Id);

            // If pageNumber and rowsPerPage are both 0, return all records
            if (pageNumber == 0 && rowsPerPage == 0)
            {
                var shoppingList = await _context.ShoppingLists
                    .Where(b => b.ShoppingId == Id)
                    .Select(sl => new DetailShoppingList
                    {
                        Id = sl.Id,
                        ItemName = sl.ItemName,
                        Brand = sl.Brand,
                        Quantity = sl.Quantity,
                        Amount = sl.Amount,
                        SubTotal = sl.Subtotal, // Assuming Subtotal is already calculated in the entity
                        CategoryId = sl.CategoryId ?? 0, // Provide default value if null
                        LocationId = sl.LocationId ?? 0 // Provide default value if null
                    })
                    .ToListAsync();

                // Optionally map category and location names if needed
                return Ok(new PagedResponse<IEnumerable<DetailShoppingList>>(shoppingList, 1, shoppingList.Count, totalRecords));
            }

            // Calculate the paginated shopping list
            var shoppingListPaged = await _context.ShoppingLists
                .Where(b => b.ShoppingId == Id)
                .Select(sl => new DetailShoppingList
                {
                    Id = sl.Id,
                    ItemName = sl.ItemName,
                    Brand = sl.Brand,
                    Quantity = sl.Quantity,
                    Amount = sl.Amount,
                    SubTotal = sl.Subtotal, // Assuming Subtotal is already calculated in the entity
                    CategoryId = sl.CategoryId ?? 0, // Provide default value if null
                    LocationId = sl.LocationId ?? 0 // Provide default value if null
                })
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();

            // Create the paginated response
            var pagedResponse = new PagedResponse<IEnumerable<DetailShoppingList>>(
                shoppingListPaged,
                pageNumber,
                rowsPerPage,
                totalRecords
            );

            // Construct URIs for pagination metadata
            var baseUri = $"{Request.Scheme}://{Request.Host}/api/Shopping/List";
            pagedResponse.FirstPage = new Uri($"{baseUri}?Id={Id}&pageNumber=1&rowsPerPage={rowsPerPage}");
            pagedResponse.LastPage = new Uri($"{baseUri}?Id={Id}&pageNumber={pagedResponse.TotalPages}&rowsPerPage={rowsPerPage}");
            pagedResponse.NextPage = pageNumber < pagedResponse.TotalPages
                ? new Uri($"{baseUri}?Id={Id}&pageNumber={pageNumber + 1}&rowsPerPage={rowsPerPage}")
                : null;
            pagedResponse.PreviousPage = pageNumber > 1
                ? new Uri($"{baseUri}?Id={Id}&pageNumber={pageNumber - 1}&rowsPerPage={rowsPerPage}")
                : null;

            return Ok(pagedResponse);
        }




        //api/Shopping/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Shopping>> GetShoppingById(int id)
        {
            try
            {
                var list = await _context.Shoppings.Include(b => b.ShoppingLists)
                                                            .FirstOrDefaultAsync(b => b.Id == id);
                if (list == null)
                    return NotFound();

                return Ok(list);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //api/Shopping
        //add finance evaluation
        [HttpPost]
        public async Task<ActionResult<Shopping>> PostShopping(Shopping Shopping)
        {
            _context.Shoppings.Add(Shopping);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingById", new { id = Shopping.Id }, Shopping);
        }


        //api/Shopping
        //update finance eva. 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShopping(int id, Shopping shopping)
        {
            if (id != shopping.Id)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(shopping).Entity.UpdatedOn = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!shoppingExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok();
        }

        // API route to change the state of a record 
        [HttpPut("ChangeState")]
        public async Task<IActionResult> ChangeStateShopping(int id)
        {
            try
            {
                // Find the shopping record by ID
                var record = await _context.Shoppings.FirstOrDefaultAsync(b => b.Id == id);

                if (record == null)
                {
                    return NotFound();
                }

                // Disable the shopping record by setting its State to 0 (inactive)
                record.State = record.State == 1 ? 0 : 1;
                record.UpdatedOn = DateTime.Now;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShopping(int id)
        {
            var finance = await _context.Shoppings.FindAsync(id);
            if (finance == null)
            {
                return NotFound();
            }

            _context.Shoppings.Remove(finance);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool shoppingExists(int id)
        {
            return _context.Shoppings.Any(b => b.Id == id);
        }

    }
}

using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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
        public async Task<ActionResult<PagedResponse<IEnumerable<ShoppingDTO>>>> GetShopping(string userId, int pageNumber = 1, int rowsPerPage = 10)
        {
            // Calculate total count of distinct shopping records
            var totalRecords = await _context.Shoppings
                .Where(r => r.UserId == userId && r.State == 1)
                .CountAsync();

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
                .Where(s => s.Shopping.UserId == userId && s.Shopping.State == 1) // Filter before projection
                .Select(s => new ShoppingDTO
                {
                    Id = s.Shopping.Id,
                    ModifiedOn = s.Shopping.UpdatedOn ?? s.Shopping.CreatedOn,
                    Description = s.Shopping.Description,
                    State = s.Shopping.State,
                    GrandTotal = s.GrandTotal
                })
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();


            // Create the paginated response
            var pagedResponse = new PagedResponse<IEnumerable<ShoppingDTO>>(
                shoppingData,
                pageNumber,
                rowsPerPage,
                totalRecords
            );


            // Construct URIs for pagination metadata
            var baseUri = $"{Request.Scheme}://{Request.Host}/api/Shopping/List?userId={userId}&";
            pagedResponse.FirstPage = new Uri($"{baseUri}pageNumber=1&rowsPerPage={rowsPerPage}");
            pagedResponse.LastPage = new Uri($"{baseUri}pageNumber={pagedResponse.TotalPages}&rowsPerPage={rowsPerPage}");
            pagedResponse.NextPage = pageNumber < pagedResponse.TotalPages
                ? new Uri($"{baseUri}pageNumber={pageNumber + 1}&rowsPerPage={rowsPerPage}")
                : null;
            pagedResponse.PreviousPage = pageNumber > 1
                ? new Uri($"{baseUri}pageNumber={pageNumber - 1}&rowsPerPage={rowsPerPage}")
                : null;

            return Ok(pagedResponse);
        }

        //api/Shopping/List
        [HttpGet("List")]
        public async Task<ActionResult> GetShoppingList(int Id, int pageNumber = 0, int rowsPerPage = 0)
        {
            // Retrieve the total record count for the specified ShoppingId
            var totalRecords = await _context.ShoppingLists.CountAsync(b => b.ShoppingId == Id);

            // Retrieve shopping header information
            var shoppInfo = await _context.Shoppings
                .Where(b => b.Id == Id)
                .Select(s => new
                {
                    s.Id,
                    s.Description,
                    ModifiedOn = s.UpdatedOn ?? s.CreatedOn,
                    s.State
                })
                .FirstOrDefaultAsync();

            if (shoppInfo == null)
            {
                return NotFound(new { Message = "Shopping information not found." });
            }

            // If pageNumber and rowsPerPage are both 0, return all records without pagination
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
                        SubTotal = sl.Subtotal,
                        CategoryId = sl.CategoryId ?? 0,
                        Category = sl.Category != null ? sl.Category.Name : null, 
                        LocationId = sl.LocationId ?? 0,
                        Location = sl.Locations != null? sl.Locations.Name : null
                    })
                    .ToListAsync();

                return Ok(new
                {
                    ShoppingData = shoppInfo,
                    Data = shoppingList,
                    Pagination = new
                    {
                        CurrentPage = 1,
                        RowsPerPage = shoppingList.Count,
                        TotalRecords = totalRecords,
                        TotalPages = 1,
                        FirstPage = string.Empty,
                        LastPage = string.Empty,
                        NextPage = string.Empty,
                        PreviousPage = string.Empty
                    }
                });
            }

            // Calculate paginated results
            var shoppingListPaged = await _context.ShoppingLists
                .Where(b => b.ShoppingId == Id)
                .Select(sl => new DetailShoppingList
                {
                    Id = sl.Id,
                    ItemName = sl.ItemName,
                    Brand = sl.Brand,
                    Quantity = sl.Quantity,
                    Amount = sl.Amount,
                    SubTotal = sl.Subtotal,
                    CategoryId = sl.CategoryId ?? 0,
                    Category = sl.Category != null ? sl.Category.Name : null,
                    LocationId = sl.LocationId ?? 0,
                    Location = sl.Locations != null ? sl.Locations.Name : null
                })
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalRecords / rowsPerPage);

            return Ok(new
            {
                ShoppingData = shoppInfo,
                Data = shoppingListPaged,
                Pagination = new
                {
                    CurrentPage = pageNumber,
                    RowsPerPage = rowsPerPage,
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    FirstPage = $"{Request.Scheme}://{Request.Host}/api/Shopping/List?Id={Id}&pageNumber=1&rowsPerPage={rowsPerPage}",
                    LastPage = $"{Request.Scheme}://{Request.Host}/api/Shopping/List?Id={Id}&pageNumber={totalPages}&rowsPerPage={rowsPerPage}",
                    NextPage = pageNumber < totalPages
                        ? $"{Request.Scheme}://{Request.Host}/api/Shopping/List?Id={Id}&pageNumber={pageNumber + 1}&rowsPerPage={rowsPerPage}"
                        : null,
                    PreviousPage = pageNumber > 1
                        ? $"{Request.Scheme}://{Request.Host}/api/Shopping/List?Id={Id}&pageNumber={pageNumber - 1}&rowsPerPage={rowsPerPage}"
                        : null
                }
            });
        }

        //api/shopping/clone
        [HttpGet("Clone")]
        public async Task<ActionResult> GetCloneShoppingList(int id)
        {
            try
            {
                // Construct the SQL query string to execute the stored procedure
                var sql = "[dbo].[stp_CloneShoppingList] @Id";

                // Using Dapper to execute the stored procedure with parameters
                var cloneRecords = await _context.Database.GetDbConnection().QueryAsync<Shopping>(
                    sql,
                    new { Id = id } // Ensure parameter name matches the stored procedure parameter
                );

                // Assuming that the first item in cloneRecords contains the new shopping list ID
                var resultData = cloneRecords.FirstOrDefault();

                // Check if resultData is null to avoid exceptions if no data is returned
                if (resultData == null)
                {
                    return NotFound("No clone record was created.");
                }

                // Return only the ID of the cloned record
                return Ok(new { Id = resultData.Id });
            }
            catch (Exception ex)
            {
                // Return detailed error message with bad request status
                return StatusCode(400, new { message = "An error occurred while cloning the shopping list.", details = ex.Message });
            }
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
        public async Task<ActionResult<Shopping>> PostShopping([FromBody] ShoppingSaveDTO shoppingSaveDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var record = new Shopping
            {
                Description = shoppingSaveDTO.Description,
                CreatedOn = DateTime.Now,
                UserId=shoppingSaveDTO.userId,
                ShoppingLists = shoppingSaveDTO.ShoppingLists?.Select(list => new ShoppingList 
                {
                    CategoryId = list.CategoryId,
                    LocationId = list.LocationId,
                    ItemName = list.ItemName,
                    Brand = list.Brand,
                    Quantity = list.Quantity,
                    Amount = list.Amount
                }).ToList()
            };

            _context.Shoppings.Add(record);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            {
                // Log the error (uncomment ex variable name and write a log.)
                return StatusCode(500, "An error occurred while saving the shopping data. Please try again later.");
            }
           

            return CreatedAtAction("GetShoppingById", new { id = record.Id }, record);
        }


        //api/Shopping
        //update finance eva. 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShopping(int id, ShoppingSaveDTO shopping)
        {
            // Validate if the provided ID matches the ID in the Shopping DTO
            if (id != shopping.Id)
            {
                return BadRequest("ID mismatch between route and body.");
            }

            try
            {

                var record = await _context.Shoppings.Include(b => b.ShoppingLists)
                                                     .FirstOrDefaultAsync(b => b.Id == id);
                if (record == null)
                {
                    return NotFound($"Budget with ID {id} not found.");
                }

                //map the records 
                record.Id = shopping.Id;
                record.Description = shopping.Description;
                record.UpdatedOn = DateTime.Now;

                //remove items 
                if (record.ShoppingLists != null)
                {
                    _context.ShoppingLists.RemoveRange(record.ShoppingLists);
                }

                //map items 
                record.ShoppingLists = shopping.ShoppingLists?.Select(list => new ShoppingList 
                { 
                    CategoryId = list.CategoryId,
                    LocationId = list.LocationId,
                    ItemName =list.ItemName,
                    Brand =list.Brand,
                    Quantity =list.Quantity,
                    Amount =list.Amount
                }).ToList();

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

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
        public async Task<ActionResult<IEnumerable<Shopping>>> GetShopping()
        {
            var Shopping = await _context.Shoppings.ToListAsync();

            return Ok(Shopping);
        }

        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<Shopping>>> GetShoppingList(int PageNumber, int RowsPage)
        {
            var sql = "EXEC [dbo].[Stp_getShoppinglist] @PageNumber, @RowsPage";

            // Using Dapper for more efficient data retrieval
            var shoppingList = await _context.Database.GetDbConnection().QueryAsync<Shopping>(sql, new { PageNumber = PageNumber, RowsPage = RowsPage });

            return Ok(shoppingList);
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

            _context.Entry(shopping).State = EntityState.Modified;

            try
            {
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

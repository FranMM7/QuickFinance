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
    public class LocationController : ControllerBase
    {
        private readonly FinanceContext _context;

        public LocationController(FinanceContext context)
        {
            _context = context;
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<Locations>> GetLocationByID(int id)
        {
            var Locations = await _context.Locations.FirstOrDefaultAsync(b => b.Id == id);

            if (Locations == null)
            {
                return NotFound();
            }

            return Locations;
        }

        [HttpPost]
        public async Task<ActionResult<Locations>> PostLocations(Locations Locations)
        {
            _context.Locations.Add(Locations);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocationByID", new { id = Locations.Id }, Locations);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocations(int id, Locations Locations)
        {
            if (id != Locations.Id)
            {
                return BadRequest();
            }

            _context.Entry(Locations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationsExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocations(int id)
        {
            var Locations = await _context.Locations.FindAsync(id);
            if (Locations == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(Locations);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool LocationsExists(int id)
        {
            return _context.Locations.Any(b => b.Id == id);
        }
    }
}

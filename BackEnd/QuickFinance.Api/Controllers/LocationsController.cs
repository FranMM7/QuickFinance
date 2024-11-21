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

        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<Locations>>> GetLocationList(string userId)
        {
            if (userId == null)
            {
                return BadRequest("User Id is require");
            }
            var locations = await _context.Locations
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return locations;
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
        public async Task<ActionResult<Locations>> PostLocations(LocationDTO Locations)
        {
            var newLocation = new Locations
            {
                Name = Locations.Name,
                UserId = Locations.UserId,
                CreatedOn = DateTime.Now,
                State = 1
            };
            _context.Locations.Add(newLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocationByID", new { id = Locations.Id }, Locations);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocations(int id, LocationDTO Locations)
        {
            if (id != Locations.Id)
            {
                return BadRequest();
            }

            var record = await _context.Locations.FirstOrDefaultAsync(r => r.Id == id);

            if (record == null)
                return BadRequest("Location not found unable to update.");

            record.Name = Locations.Name;
            record.UpdatedOn = DateTime.Now;

            _context.Entry(record).State = EntityState.Modified;

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

        // API route to change the state of a record 
        [HttpPut("ChangeState")]
        public async Task<IActionResult> ChangeStateLocation(int id)
        {
            try
            {
                // Find the record by ID
                var record = await _context.Locations.FirstOrDefaultAsync(b => b.Id == id);

                if (record == null)
                {
                    return NotFound();
                }

                // Disable the record by setting its State to 0 (inactive)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocations(int id)
        {
            var Locations = await _context.Locations.FindAsync(id);
            if (Locations == null)
            {
                return NotFound();
            }

            var recordInUse = await _context.ShoppingLists.CountAsync(r => r.LocationId == id);
            if (recordInUse > 0)
                return BadRequest("Unable to delete record because is been refer by another record");

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

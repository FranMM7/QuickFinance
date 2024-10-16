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
    public class FinanceEvaluationController : ControllerBase
    {
        private readonly FinanceContext _context;

        public FinanceEvaluationController(FinanceContext context)
        {
            _context = context;
        }

        //api/FinanceEvaluation/List
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<DetailFinanceList>>> GetFinanceEvaluationList(int PageNumber, int RowsPage)
        {
            var sql = "EXEC [DBO].[stp_getfinanceEvaluations]";
            var list = await _context.Database.GetDbConnection().ExecuteScalarAsync<DetailFinanceList>(sql, new {PageNumber=PageNumber, RowsPage=RowsPage});
        
            return Ok(list);
        }

        //api/FinanceEvaluation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinanceEvaluation>>> GetFinanceEvaluation()
        {
            var financeEvaluation = await _context.FinanceEvaluations.ToListAsync();

            return Ok(financeEvaluation);
        }

        //api/FinanceEvaluation/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FinanceEvaluation>> GetFinanceEvaluationById(int id)
        {
            try
            {
                var list = await _context.FinanceEvaluations.Include(b => b.FinanceDetails)
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

        //api/FinanceEvaluation
        //add finance evaluation
        [HttpPost]
        public async Task<ActionResult<FinanceEvaluation>> PostFinanceEvaluation(FinanceEvaluation financeEvaluation)
        {
            _context.FinanceEvaluations.Add(financeEvaluation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinanceEvaluationById", new { id = financeEvaluation.Id }, financeEvaluation);
        }


        //api/FinanceEvaluation
        //update finance eva. 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinanceEvaluation(int id, FinanceEvaluation finance)
        {
            if (id != finance.Id)
            {
                return BadRequest();
            }

            _context.Entry(finance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!financeEvaExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok();
        }

        // API route to change the state of a record 
        [HttpPut("ChangeState")]
        public async Task<IActionResult> ChangeStateFinanceEvaluation(int id)
        {
            try
            {
                // Find the record by ID
                var record = await _context.FinanceEvaluations.FirstOrDefaultAsync(b => b.Id == id);

                if (record == null)
                {
                    return NotFound();
                }

                // Disable the record by setting its State to 0 (inactive) or 1 (active)
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
        public async Task<IActionResult> DeleteFinanceEvaluation(int id)
        {
            var finance = await _context.FinanceEvaluations.FindAsync(id);
            if (finance == null)
            {
                return NotFound();
            }

            _context.FinanceEvaluations.Remove(finance);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool financeEvaExists(int id)
        {
            return _context.FinanceEvaluations.Any(b => b.Id == id);
        }

    }
}

using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
        public async Task<ActionResult<PagedResponse<IEnumerable<DetailFinanceList>>>> GetFinanceEvaluationList(int pageNumber = 1, int rowsPerPage = 10)
        {
            // Construct the SQL query string to execute the stored procedure
            var sql = "EXEC [DBO].[stp_getfinanceEvaluations] @PageNumber, @RowsPage"; // Added parameters for pagination

            // Using Dapper for efficient data retrieval of Finance details
            var list = await _context.Database.GetDbConnection().QueryAsync<DetailFinanceList>(sql, new { PageNumber = pageNumber, RowsPage = rowsPerPage });

            // Count total records in the database that are active (State == 1)
            var totalRecords = await _context.FinanceEvaluations.CountAsync(b => b.State == 1);

            // Create the paged response with the list of finance evaluations
            var pagedResponse = new PagedResponse<IEnumerable<DetailFinanceList>>(
                list, // The list of finance evaluation details
                pageNumber, // Current page number
                rowsPerPage, // Number of rows per page
                totalRecords // Total records in the database
            );

            // Construct URIs for pagination metadata
            var baseUri = $"{Request.Scheme}://{Request.Host}/api/FinanceEvaluation/List"; // Corrected the base URI to point to FinanceEvaluation
            pagedResponse.FirstPage = new Uri($"{baseUri}?pageNumber=1&rowsPerPage={rowsPerPage}");
            pagedResponse.LastPage = new Uri($"{baseUri}?pageNumber={pagedResponse.TotalPages}&rowsPerPage={rowsPerPage}");
            pagedResponse.NextPage = pageNumber < pagedResponse.TotalPages
                ? new Uri($"{baseUri}?pageNumber={pageNumber + 1}&rowsPerPage={rowsPerPage}")
                : null;
            pagedResponse.PreviousPage = pageNumber > 1
                ? new Uri($"{baseUri}?pageNumber={pageNumber - 1}&rowsPerPage={rowsPerPage}")
                : null;

            // Return the paged response with the finance evaluation details
            return Ok(pagedResponse);
        }


        //api/FinanceEvaluation
        [HttpGet]
        public async Task<ActionResult<FinanceEvaluation>> GetFinanceEvaluation()
        {
            var financeEvaluation = await _context.FinanceEvaluations
                .Include(b => b.FinanceDetails)
                .OrderByDescending(fe => fe.CreatedOn) 
                .FirstOrDefaultAsync();

            if (financeEvaluation == null)
            {
                return NotFound(new { Message = "No finance evaluations found." });
            }

            return Ok(financeEvaluation);
        }

        [HttpGet("Exists")]
        public async Task<ActionResult<Boolean>> getHasARecord()
        {
            var records = await _context.FinanceEvaluations.CountAsync();

            bool hasARecord = records > 0? true: false;

            return Ok(hasARecord);

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

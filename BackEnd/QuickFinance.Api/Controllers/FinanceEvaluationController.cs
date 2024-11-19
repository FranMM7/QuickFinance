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
        public async Task<ActionResult<PagedResponse<IEnumerable<FinanceList>>>> GetFinanceEvaluationList(string userId, int pageNumber = 1, int rowsPerPage = 10)
        {
            // Construct the SQL query string to execute the stored procedure
            var sql = "EXEC [DBO].[stp_getfinanceEvaluations] @userId, @PageNumber, @RowsPage"; // Added parameters for pagination

            // Using Dapper for efficient data retrieval of Finance details
            var list = await _context.Database.GetDbConnection().QueryAsync<FinanceList>(sql, new { userId= userId, PageNumber = pageNumber, RowsPage = rowsPerPage });

            // Count total records in the database that are active (State == 1)
            var totalRecords = await _context.FinanceEvaluations.CountAsync(b => b.State == 1);

            // Create the paged response with the list of finance evaluations
            var pagedResponse = new PagedResponse<IEnumerable<FinanceList>>(
                list, // The list of finance evaluation details
                pageNumber, // Current page number
                rowsPerPage, // Number of rows per page
                totalRecords // Total records in the database
            );

            // Construct URIs for pagination metadata
            var baseUri = $"{Request.Scheme}://{Request.Host}/api/FinanceEvaluation/List?userId={userId}&"; // Corrected the base URI to point to FinanceEvaluation
            pagedResponse.FirstPage = new Uri($"{baseUri}pageNumber=1&rowsPerPage={rowsPerPage}");
            pagedResponse.LastPage = new Uri($"{baseUri}pageNumber={pagedResponse.TotalPages}&rowsPerPage={rowsPerPage}");
            pagedResponse.NextPage = pageNumber < pagedResponse.TotalPages
                ? new Uri($"{baseUri}pageNumber={pageNumber + 1}&rowsPerPage={rowsPerPage}")
                : null;
            pagedResponse.PreviousPage = pageNumber > 1
                ? new Uri($"{baseUri}pageNumber={pageNumber - 1}&rowsPerPage={rowsPerPage}")
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
                .Include(b => b.FinancesIncomes)
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
                var record = await _context.FinanceEvaluations.Include(b => b.FinanceDetails)
                                                              .Include(b => b.FinancesIncomes)
                                                              .OrderByDescending(fe => fe.CreatedOn)
                                                              .FirstOrDefaultAsync(b => b.Id == id);
                if (record == null)
                    return NotFound(new { Message = "No finance evaluations found." });

                return Ok(record);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //api/FinanceEvaluation
        [HttpPost]
        public async Task<ActionResult<FinanceEvaluation>> PostFinanceEvaluation([FromBody] FinanceDTO finance)
        {
            // Validate the model automatically if [ApiController] is not applied
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map incoming FinanceDTO to FinanceEvaluation entity
            var newRecord = new FinanceEvaluation
            {
                Title = finance.Title,
                CreatedOn = DateTime.UtcNow, // Use UTC for standardization
                TotalIncomes = finance.FinanceIncomes?.Sum(b => b.Amount) ?? 0,
                TotalExpenses = finance.FinanceDetails?.Sum(b => b.Amount) ?? 0,
                FinanceDetails = finance.FinanceDetails?.Select(details => new FinanceDetail
                {
                    Description = details.Description,
                    Amount = details.Amount,
                    CategoryId = details.CategoryId,
                    ExpenseCategory = details.ExpenseCategory
                }).ToList(),
                FinancesIncomes = finance.FinanceIncomes?.Select(incomes => new FinanceIncome
                {
                    Description = incomes.Description,
                    Amount = incomes.Amount
                }).ToList()
            };

            // Add the new record to the context
            _context.FinanceEvaluations.Add(newRecord);

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Optionally, log exception for debugging purposes
                return StatusCode(500, "An error occurred while saving the Finance Evaluation.");
            }

            // Return minimal details or a DTO instead of the entire entity
            return CreatedAtAction("GetFinanceEvaluationById", new { id = newRecord.Id }, new { newRecord.Id, newRecord.Title });
        }




        //api/FinanceEvaluation/{id}
        //update finance evaluation
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinanceEvaluation(int id, [FromBody] FinanceDTO finance)
        {
            if (id != finance.Id)
            {
                return BadRequest("ID mismatch between route and body.");
            }

            // Check if the record exists
            var record = await _context.FinanceEvaluations
                                       .Include(b => b.FinanceDetails)
                                       .Include(b => b.FinancesIncomes)
                                       .FirstOrDefaultAsync(b => b.Id == id);

            if (record == null)
            {
                return NotFound($"Record with ID {id} not found.");
            }

            // Map updated values
            record.Title = finance.Title;
            record.UpdatedOn = DateTime.UtcNow;

            // Start a transaction for atomicity
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Remove existing finance details
                if (record.FinanceDetails != null)
                {
                    _context.FinanceDetails.RemoveRange(record.FinanceDetails);
                }

                if (record.FinancesIncomes != null)
                {
                    _context.FinanceIncomes.RemoveRange(record.FinancesIncomes);
                }

                // Map new finance details
                record.FinanceDetails = finance.FinanceDetails?.Select(details => new FinanceDetail
                {
                    Description = details.Description,
                    Amount = details.Amount,
                    CategoryId = details.CategoryId,
                    ExpenseCategory = details.ExpenseCategory
                }).ToList();

                record.FinancesIncomes = finance.FinanceIncomes?.Select(incomes => new FinanceIncome
                {
                    Description = incomes.Description,
                    Amount = incomes.Amount
                }).ToList();

                // Save changes
                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // Commit transaction
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!financeEvaExists(id))
                {
                    return NotFound($"Budget with ID {id} not found during update.");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred while updating the Finance Evaluation: {ex.Message}");
            }

            // Return a DTO with minimal details or confirmation response
            return Ok(new { record.Id, record.Title, record.UpdatedOn });
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

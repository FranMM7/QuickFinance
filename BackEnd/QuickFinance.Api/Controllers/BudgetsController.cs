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
    public class BudgetsController : ControllerBase
    {
        private readonly FinanceContext _context;

        public BudgetsController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("BudgetsInfo")]
        public async Task<ActionResult<string>> GetBudgetInfo(string userId)
        {
            if (userId == null)
                return BadRequest("User Id is required");

            var sql = "[dbo].[stp_GetBudgetOverviewJSON] @userId";

            // Use Dapper's ExecuteScalarAsync to execute the stored procedure and return the JSON
            var result = await _context.Database.GetDbConnection().ExecuteScalarAsync<string>(sql, new { userId=userId });

            // Return the result as an Ok response
            return Ok(result);
        }

        //api/budgets/list
        [HttpGet("List")]
        public async Task<ActionResult<PagedResponse<IEnumerable<DetailBudgetList>>>> GetBudgetList(string userId, int pageNumber = 1, int rowsPerPage = 10)
        {
            if (userId == null)
                return BadRequest("User Id is required");

            // Construct the SQL query string to execute the stored procedure
            var sql = "EXEC [dbo].[stp_GetBudgetDetails] @userId, @PageNumber, @RowsPage";

            // Count total records for the given user and active state (State == 1)
            var totalRecords = await _context.Budgets
                .Where(r => r.UserId == userId && r.State == 1)
                .CountAsync();

            // Using Dapper for efficient data retrieval of budget details
            var budgetList = await _context.Database.GetDbConnection().QueryAsync<DetailBudgetList>(
                sql,
                new { userId = userId, PageNumber = pageNumber, RowsPage = rowsPerPage } // Passing parameters for pagination
            );

            // Create the pagination response
            var pagedResponse = new PagedResponse<IEnumerable<DetailBudgetList>>(
                budgetList, // The list of budget details
                pageNumber, // Current page number
                rowsPerPage, // Number of rows per page
                totalRecords // Total records in the database
            );

            // Construct URIs for pagination metadata
            var baseUri = $"{Request.Scheme}://{Request.Host}/api/Budgets/List";
            pagedResponse.FirstPage = new Uri($"{baseUri}?pageNumber=1&rowsPerPage={rowsPerPage}");
            pagedResponse.LastPage = new Uri($"{baseUri}?pageNumber={pagedResponse.TotalPages}&rowsPerPage={rowsPerPage}");
            pagedResponse.NextPage = pageNumber < pagedResponse.TotalPages
                ? new Uri($"{baseUri}?pageNumber={pageNumber + 1}&rowsPerPage={rowsPerPage}")
                : null;
            pagedResponse.PreviousPage = pageNumber > 1
                ? new Uri($"{baseUri}?pageNumber={pageNumber - 1}&rowsPerPage={rowsPerPage}")
                : null;

            // Return the paged response with the budget details
            return Ok(pagedResponse);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Budget>> GetBudget(int id)
        {
            var budget = await _context.Budgets.Include(b => b.Expenses)
                                                 .FirstOrDefaultAsync(b => b.Id == id);

            if (budget == null)
            {
                return NotFound();
            }

            return budget;
        }

        [HttpPost]
        public async Task<ActionResult<Budget>> PostBudget([FromBody] BudgetDto budgetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the DTO to the Budget entity
            var budget = new Budget
            {
                Title = budgetDto.Title,
                TotalAllocatedBudget = budgetDto.TotalAllocatedBudget,
                State = budgetDto.State,
                CreatedOn = DateTime.UtcNow, // Set the CreatedOn property

                // Map ExpenseDto list to Expense list
                Expenses = budgetDto.ExpensesDTO?.Select(expenseDto => new Expense
                {
                    Description = expenseDto.Description,
                    Amount = expenseDto.Amount,
                    IsExecuted = expenseDto.IsExecuted,
                    ExpenseDueDate = expenseDto.ExpenseDueDate,
                    CategoryId = expenseDto.CategoryId,
                    PaymentMethodId = expenseDto.PaymentMethodId,
                    // BudgetId can be assigned later after saving the budget
                }).ToList() // Convert List<ExpenseDto> to List<Expense>
            };

            _context.Budgets.Add(budget);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error (uncomment ex variable name and write a log.)
                return StatusCode(500, "An error occurred while saving the budget. Please try again later.");
            }

            // Return the created budget entity, including auto-generated properties
            return CreatedAtAction("GetBudget", new { id = budget.Id }, budget);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudget(int id, [FromBody] BudgetDto budget)
        {
            // Validate if the provided ID matches the ID in the budget DTO
            if (id != budget.Id)
            {
                return BadRequest("ID mismatch between route and body.");
            }

            // Check if the budget exists before attempting to modify it
            var record = await _context.Budgets.Include(b => b.Expenses).FirstOrDefaultAsync(b => b.Id == id);
            if (record == null)
            {
                return NotFound($"Budget with ID {id} not found.");
            }

            // Map the values from BudgetDto to the existing Budget entity
            record.Title = budget.Title; // Example of mapping a property
            record.TotalAllocatedBudget = budget.TotalAllocatedBudget; // Map other properties as needed
            record.UpdatedOn = DateTime.UtcNow; // Update the modification date

            // Remove existing expenses if any
            if (record.Expenses != null)
            {
                _context.Expenses.RemoveRange(record.Expenses);
            }

            // Map the new expenses
            record.Expenses = budget.ExpensesDTO?.Select(expenseDto => new Expense
            {
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                IsExecuted = expenseDto.IsExecuted,
                ExpenseDueDate = expenseDto.ExpenseDueDate,
                CategoryId = expenseDto.CategoryId,
                PaymentMethodId = expenseDto.PaymentMethodId,
            }).ToList(); // Convert List<ExpenseDto> to List<Expense>

            // Attempt to save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues
                if (!BudgetExists(id))
                {
                    return NotFound($"Budget with ID {id} not found during update.");
                }
                throw; // Rethrow the exception if it's a different concurrency issue
            }

            return Ok(record); // Return the updated budget or a relevant response
        }

        // API route to change the state of a record 
        [HttpPut("ChangeState")]
        public async Task<IActionResult> ChangeStateBudget(int id)
        {
            try
            {
                // Find the record by ID
                var record = await _context.Budgets.FirstOrDefaultAsync(b => b.Id == id);

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
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool BudgetExists(int id)
        {
            return _context.Budgets.Any(b => b.Id == id);
        }
    }
}

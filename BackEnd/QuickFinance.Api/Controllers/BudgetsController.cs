﻿using Dapper;
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
        public async Task<ActionResult<string>> GetBudgetInfo()
        {
            var sql = "EXEC [DBO].[sp_GetBudgetOverviewJSON]";

            // Use Dapper's ExecuteScalarAsync to execute the stored procedure and return the JSON
            var result = await _context.Database.GetDbConnection().ExecuteScalarAsync<string>(sql);

            // Return the result as an Ok response
            return Ok(result);
        }

        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<DetailBudgetList>>> GetBudgetList(int PageNumber, int RowsPage)
        {
            var sql = "EXEC [dbo].[GetBudgetDetails] @PageNumber, @RowsPage";

            // Using Dapper for more efficient data retrieval
            var budgetSummaries = await _context.Database.GetDbConnection().QueryAsync<DetailBudgetList>(sql, new {PageNumber= PageNumber, RowsPage=RowsPage });

            return Ok(budgetSummaries);
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
        public async Task<IActionResult> PutBudget(int id, Budget budget)
        {
            if (id != budget.Id)
            {
                return BadRequest();
            }

            _context.Entry(budget).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok();
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

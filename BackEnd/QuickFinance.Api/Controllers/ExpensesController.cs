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
    public class ExpensesController : ControllerBase
    {
        private readonly FinanceContext _context;

        public ExpensesController(FinanceContext context)
        {
            _context = context;
        }

        [HttpGet("List/{budgetId}")]
        public async Task<ActionResult<IEnumerable<DetailExpensesList>>> GetExpensesList(string userId,int budgetId, int PageNumber, int RowsPage)
        {
            // Parameterized query to avoid SQL injection
            var sql = "EXEC [dbo].[stp_GetExpenseDetails] @userId, @BudgetId, @PageNumber, @RowsPage";

            // Execute the stored procedure with the parameter with dapper
            var expenses = await _context.Database.GetDbConnection().QueryAsync<DetailExpensesList>(sql, new { userId= userId, BudgetId = budgetId, PageNumber=PageNumber, RowsPage=RowsPage});

            return Ok(expenses);
        }



        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            return await _context.Expenses.Include(e => e.Category).Include(e => e.Budget).ToListAsync();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var expense = await _context.Expenses.Include(e => e.Category).Include(e => e.Budget)
                                                 .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        // POST: api/Expenses
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpense", new { id = expense.Id }, expense);
        }

        // PUT: api/Expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                _context.Entry(expense).Entity.UpdatedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok();
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return Ok();
        }        

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}

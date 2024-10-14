using System;
using System.Collections.Generic;

namespace QuickFinance.Api.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string Title { get; set; } // Requied - rename from month to title
        public decimal TotalBudget { get; set; } // Required, type money

        // Navigation property for related Expenses
        public List<Expense> Expenses { get; set; }
    }
}

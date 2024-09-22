using System;

namespace QuickFinance.Api.Models
{
    public class BudgetLimit
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public int CategoryId { get; set; } // Foreign key to Category
        public Category Category { get; set; }

        public decimal BudgetLimitAmount { get; set; } // Budget limit amount for the category
    }
}

using System;

namespace QuickFinance.Api.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } // Auto-populated by the database
        public DateTime UpdatedOn { get; set; } // Auto-updated by the database

        public int BudgetId { get; set; } // Foreign key to Budget
        public virtual Budget Budget { get; set; } // Navigation property

        public string Description { get; set; } // Required
        public int CategoryId { get; set; } // Foreign key to Category
        public virtual Category Category { get; set; } // Navigation property

        public DateTime DueDate { get; set; }
        public int PaymentMethodId { get; set; } // Foreign key to PaymentMethod
        public virtual PaymentMethod PaymentMethod { get; set; } // Navigation property

        public decimal Amount { get; set; } // Expense amount

        public bool Executed { get; set; } // Determines if the payment has been applied or not. 
    }
}

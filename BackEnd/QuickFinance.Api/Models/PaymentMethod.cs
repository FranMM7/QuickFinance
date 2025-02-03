using System;

namespace QuickFinance.Api.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now; // Auto-populated by the database
        public DateTime? UpdatedOn { get; set; }

        public string PaymentMethodName { get; set; } = ""; // Required, e.g., "Credit Card", "Cash"
        public int State { get; set; } = 1; //1=active, 0=inactive
    }
}

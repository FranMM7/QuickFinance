using System;

namespace QuickFinance.Api.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string Name { get; set; } // Required, e.g., "Credit Card", "Cash"
    }
}

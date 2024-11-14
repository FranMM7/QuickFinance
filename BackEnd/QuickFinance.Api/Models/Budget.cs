using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickFinance.Api.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string Title { get; set; } // Requied - rename from month to title
        public decimal TotalAllocatedBudget { get; set; } // Required, type money
        public int State { get; set; } = 1; //1=active, 0=inactive

        public string UserId { get; set; }
        // Navigation property for related Expenses
        public List<Expense> Expenses { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }  // Navigation property
    }


}

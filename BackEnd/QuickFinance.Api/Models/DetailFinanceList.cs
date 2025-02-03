using System;
using System.Collections.Generic;

namespace QuickFinance.Api.Models
{
    public class DetailFinanceList
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Category { get; set; }
        public string Description { get; set; }
        public string ExpenseType { get; set; }
        public decimal Amount { get; set; }
    }
}

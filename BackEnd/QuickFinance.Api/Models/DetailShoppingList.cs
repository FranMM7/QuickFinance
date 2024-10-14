using System;
using System.Collections.Generic;

namespace QuickFinance.Api.Models
{
    public class DetailShoppingList
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string Description { get; set; }
        public string Item { get; set; }
        public int qty { get; set; }
        public decimal Amount { get; set; }
        public decimal subTotal { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
    }
}

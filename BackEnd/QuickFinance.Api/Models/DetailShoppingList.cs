using System;
using System.Collections.Generic;

namespace QuickFinance.Api.Models
{
    public class DetailShoppingList
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Brand { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal SubTotal { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; }
    }
}

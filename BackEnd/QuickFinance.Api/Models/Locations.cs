using System;
using System.Collections.Generic;

namespace QuickFinance.Api.Models
{
    public class Locations
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuickFinance.Api.Models
{
    public class Locations
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Name { get; set; }
        public int State { get; set; } = 1; //1=active, 0=inactive
        public string UserId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }  // Navigation property
    }
}

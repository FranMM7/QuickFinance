using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuickFinance.Api.Models
{
    public class FinanceIncome
    {
        public int Id { get; set; }
        public int FinanceId { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]  // Specify decimal type and precision
        public decimal Amount { get; set; }

        [JsonIgnore]
        public FinanceEvaluation FinanceEvaluation { get; set; }

    }
}

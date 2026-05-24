using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UnityVentures.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Business Business { get; set; } = null!;
    }
}

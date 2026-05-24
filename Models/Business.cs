using Microsoft.AspNetCore.Identity;
namespace UnityVentures.Models
{
    public class Business
    {
        public int BusinessId { get; set; }
        public decimal Balance { get; set; }
        public string BusinessName { get; set; } = null!;
        public string IdNumber { get; set; } = null!;
        public string IdCardWithSelfieImageUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public IdentityUser User { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}

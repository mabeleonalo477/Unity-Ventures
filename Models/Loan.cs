namespace UnityVentures.Models
{
    public class Loan
    {
        public int LoanId { get; set; }
        public decimal AmountLoaned { get; set; }
        public bool IsPaid { get; set; }

        public DateTime CreatedAt { get; set; }

        public Business Business { get; set; } = null!;
    }
}

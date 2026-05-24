public class BusinessInsightsViewModel
{
    public string BusinessName { get; set; } = null!;

    public int Deposits { get; set; }

    public decimal TotalCashSpentRand { get; set; }

    public decimal CurrentLoanAmountRand { get; set; }

    public string CurrentLoanStatus { get; set; } = null!;

    public decimal PreviousLoanAmountRand { get; set; }

    public string PreviousLoanStatus { get; set; } = null!;

    public bool EligibleForNewLoan { get; set; }
}
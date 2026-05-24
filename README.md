# Unity-Ventures
Millions of South Africans run informal businesses (spaza shops, street hawking, taxis) using only cash. 
Bank fees are too high, apps too complex. This system empowers them to access micro‑loans without 
changing their habits.

How it works:
- A Business Deposits cash to their account using their local supermarket.
- The business can apply for a small loan on the system, only after a 5 month period from registration.
- Loan eligibility is tracked alongside cash deposit history.
- Loan Repayments happen after re-deposits via a local Supermarket(e.g., Pick n' Pay) .
- No credit checks, or forms required.

Tech stack:
- SQL view that turns cash transactions into a credit history.
- C# backend (ASP.NET Core MVC) with loan eligibility logic.
- Razor view showing current loan, previous loan, and repayment status.

Business insight: Past behaviour predicts future trust. 
The system uses only what already exists – cash receipts – to include informal workers in the formal economy.

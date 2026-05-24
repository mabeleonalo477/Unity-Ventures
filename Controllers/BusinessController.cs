using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Transactions;
using UnityVentures.Data;
using UnityVentures.Models;
using UnityVentures.ViewModels;
namespace UnityVentures.Controllers
{
    public class BusinessController : Controller
    {
        private readonly UnityVenturesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BusinessController(UnityVenturesDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var info = _context.Businesses.FirstOrDefault(b => b.User.Id == user.Id);

            var Transactions = _context.transactions.ToList();

            DashboardViewModel viewModel = new()
            {
                BusinessName = info.BusinessName,
                Transactions = Transactions,
                Balance = info.Balance
            };
            return View(viewModel);
        }

        public async Task<IActionResult> RequestLoan()
        {
            var user = await _userManager.GetUserAsync(User);

            var business = _context.Businesses
                .Include(b => b.Transactions)
                .FirstOrDefault(b => b.User.Id == user.Id);

            if (business == null)
            {
                return NotFound();
            }

            var totalDeposits = business.Transactions
                .Where(t => t.Type == "Deposit")
                .Sum(t => t.Amount);

            var vm = new RequestLoanViewModel();

            bool activeForFiveMonths = business.CreatedAt <= DateTime.Now.AddMonths(-5);

            bool depositedEnough = totalDeposits >= 5000;

            if (!activeForFiveMonths || !depositedEnough)
            {
                vm.Message =
                    "You currently do not qualify for a loan. Businesses must be active for at least 5 months and have deposited a minimum of R5,000.";

                vm.AmountQualifyingFor = 0;

                return View(vm);
            }

            vm.AmountQualifyingFor = totalDeposits * 0.35m;

            return View(vm);
        }

        public async Task<IActionResult> Insights()
        {
            var user = await _userManager.GetUserAsync(User);

            var business = await _context.Businesses
                .Include(b => b.Transactions)
                .Include(b => b.Loans)
                .FirstOrDefaultAsync(b => b.User.Id == user.Id);

            if (business == null)
            {
                return NotFound();
            }

            // Cash purchases (Deposits)
            var deposits = business.Transactions
                .Where(t => t.Type == "Deposit")
                .ToList();

            int numberOfCashPurchases = deposits.Count;

            decimal totalCashSpent = deposits.Sum(t => t.Amount);

            // Loan information
            var loans = business.Loans
                .OrderByDescending(l => l.CreatedAt)
                .ToList();

            var currentLoan = loans.FirstOrDefault();

            var previousLoan = loans.Skip(1).FirstOrDefault();

            bool activeForFiveMonths =
                business.CreatedAt <= DateTime.UtcNow.AddMonths(-5);

            bool depositedEnough =
                totalCashSpent >= 5000;

            bool hasOutstandingLoan =
                currentLoan != null && !currentLoan.IsPaid;

            var vm = new BusinessInsightsViewModel
            {
                BusinessName = business.BusinessName,

                Deposits = numberOfCashPurchases,

                TotalCashSpentRand = totalCashSpent,

                CurrentLoanAmountRand =
                    currentLoan?.AmountLoaned ?? 0,

                CurrentLoanStatus =
                    currentLoan == null
                        ? "No Loan"
                        : currentLoan.IsPaid
                            ? "Paid Off"
                            : "Still Owing",

                PreviousLoanAmountRand =
                    previousLoan?.AmountLoaned ?? 0,

                PreviousLoanStatus =
                    previousLoan == null
                        ? "No Previous Loan"
                        : previousLoan.IsPaid
                            ? "Paid Off"
                            : "Still Owing",

                EligibleForNewLoan =
                    activeForFiveMonths &&
                    depositedEnough &&
                    !hasOutstandingLoan
            };

            return View(vm);
        }

        public async Task<IActionResult> Withdraw()
        {
            var user = await _userManager.GetUserAsync(User);

            var business = _context.Businesses
                .FirstOrDefault(b => b.User.Id == user.Id);

            if (business == null)
            {
                return NotFound();
            }

            var vm = new WithdrawViewModel
            {
                AvailableBalance = business.Balance
            };

            return View(vm);
        }



        [HttpPost]
        public async Task<IActionResult> Withdraw(WithdrawViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var business = await _context.Businesses
                .FirstOrDefaultAsync(b => b.User.Id == user.Id);

            if (business == null)
                return NotFound();

            if (viewModel.Amount > business.Balance)
            {
                ModelState.AddModelError("", "Insufficient balance.");
                return View(viewModel);
            }

            // Deduct balance
            business.Balance -= viewModel.Amount;

            // Create withdrawal transaction
            _context.transactions.Add(new Models.Transaction()
            {
                Amount = viewModel.Amount,
                Type = "Withdrawal",
                CreatedAt = DateTime.UtcNow,
                Business = business
            });

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Withdrawal of R{viewModel.Amount:N2} processed successfully.";

            return RedirectToAction("Dashboard");
        }


    }
}

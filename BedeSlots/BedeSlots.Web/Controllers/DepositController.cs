using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class DepositController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ITransactionService transactionService;
        private readonly ICardService cardService;
        private readonly IUserBalanceService userBalanceService;

        public DepositController(UserManager<User> userManager, IUserBalanceService userBalanceService, ITransactionService transactionService, ICardService cardService)
        {
            this.userManager = userManager;
            this.userBalanceService = userBalanceService;
            this.transactionService = transactionService;
            this.cardService = cardService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);

            var depositVM = new DepositViewModel()
            {
                Currency = user.Currency
            };

            return View(depositVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(DepositViewModel depositViewModel)
        {
            if (!ModelState.IsValid)
            {
                this.StatusMessage = "Error! The deposit is not completed.";
                return PartialView("_StatusMessage", this.StatusMessage);
            }

            var user = await this.userManager.GetUserAsync(HttpContext.User);
            var card = await this.cardService.GetCardDetailsByIdAsync(depositViewModel.BankCardId);

            var transaction = await this.transactionService.AddTransactionAsync(TransactionType.Deposit, user.Id,
                card.LastFourDigit, depositViewModel.DepositAmount, user.Currency);

            var deposit = await this.userBalanceService.DepositMoneyAsync(depositViewModel.DepositAmount, user.Id);

            string currencySymbol = WebConstants.CurrencySymbols[user.Currency];
            this.StatusMessage = $"Successfully deposited {depositViewModel.DepositAmount} {currencySymbol}.";

            return PartialView("_StatusMessage", this.StatusMessage);
        }
    }
}
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
    public class WithdrawController : Controller
    {
        private readonly IUserBalanceService userBalanceService;
        private readonly ITransactionService transactionService;
        private readonly ICardService cardService;
        private readonly ICurrencyService currencyService;
        private readonly UserManager<User> userManager;

        public WithdrawController(IUserBalanceService userBalanceService, ITransactionService transactionService, ICardService cardService, ICurrencyService currencyService, UserManager<User> userManager)
        {
            this.userBalanceService = userBalanceService;
            this.transactionService = transactionService;
            this.cardService = cardService;
            this.currencyService = currencyService;
            this.userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);

            var model = new WithdrawViewModel()
            {
                Currency = user.Currency
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(WithdrawViewModel model)
        {
            if (!ModelState.IsValid)
            {
                this.StatusMessage = "Error! The withdraw is not completed.";
                return PartialView("_StatusMessage", this.StatusMessage);
            }

            var user = await this.userManager.GetUserAsync(HttpContext.User);
            var userBalance = await this.userBalanceService.GetUserBalanceByIdAsync(user.Id);

            if (userBalance < model.Amount)
            {
                this.StatusMessage = "Error! The withdraw is not completed.";
                return PartialView("_StatusMessage", this.StatusMessage);
            }

            await this.userBalanceService.ReduceMoneyAsync(model.Amount, user.Id);

            var card = await this.cardService.GetCardNumberByIdAsync(model.BankCardId);
            var userCurrency = await this.currencyService.GetUserCurrencyAsync(user.Id);

            await this.transactionService.AddTransactionAsync(TransactionType.Withdraw,
                user.Id, card.Number, model.Amount, userCurrency);

            string currencySymbol = WebConstants.CurrencySymbols[user.Currency];
            this.StatusMessage = $"Successfully withdrawn {model.Amount} {currencySymbol}.";

            return PartialView("_StatusMessage", this.StatusMessage);
        }
    }
}
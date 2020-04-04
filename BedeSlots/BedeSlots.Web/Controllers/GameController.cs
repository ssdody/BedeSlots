
using BedeSlots.Data.Models;
using BedeSlots.Games.Contracts;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using BedeSlots.Web.Models.GameViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly ISlotMachine game;
        private readonly UserManager<User> userManager;
        private readonly ITransactionService transactionService;
        private readonly IUserBalanceService userBalanceService;

        public GameController(ISlotMachine game, ITransactionService transactionService, UserManager<User> userManager, IUserBalanceService userBalanceService)
        {
            this.game = game;
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.userBalanceService = userBalanceService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Spin(GameStakeViewModel stakeModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 299;
                this.StatusMessage = "Error! The bet is not completed.";
                return PartialView("_StatusMessage", this.StatusMessage);
            }

            var user = await userManager.GetUserAsync(HttpContext.User);
            var convertedUserBalance = await this.userBalanceService.GetUserBalanceByIdAsync(user.Id);

            decimal stake = stakeModel.Amount;
            if (stake > convertedUserBalance)
            {
                Response.StatusCode = 299;
                ModelState.AddModelError("Stake", "Error! You don't have enough money to make this bet!");
                this.StatusMessage = "Error! You don't have enough money to make this bet!";
                return PartialView("_StatusMessage", this.StatusMessage);
            }

            int rows = stakeModel.Rows;
            int cols = stakeModel.Cols;

            GameType gameType;

            if (rows == 4 && cols == 3)
            {
                gameType = GameType._4x3;
            }
            else if (rows == 5 && cols == 5)
            {
                gameType = GameType._5x5;
            }
            else if (rows == 8 && cols == 5)
            {
                gameType = GameType._8x5;
            }
            else
            {
                return this.RedirectToAction("Index");
            }

            await this.userBalanceService.ReduceMoneyAsync(stake, user.Id);
            string gameTypeString = gameType.ToString().Substring(1);

            var stakeTransaction = await this.transactionService.AddTransactionAsync(TransactionType.Stake, user.Id, gameTypeString, stake, user.Currency);

            var result = game.Spin(rows, cols, stake);

            var model = new GameSlotViewModel()
            {
                Rows = rows,
                Cols = cols,
                Matrix = result.Matrix,
                Amount = result.Amount,
                Balance = Math.Round((convertedUserBalance - stake), 2),
                Currency = user.Currency,
                WinningRows = String.Join("", result.WinningRows),
                Coefficient = result.Coefficient
            };

            if (result.Amount > 0)
            {
                var winTransaction = await this.transactionService.AddTransactionAsync(TransactionType.Win, user.Id, gameTypeString, result.Amount, user.Currency);

                await userBalanceService.DepositMoneyAsync(result.Amount, user.Id);
                model.Balance += result.Amount;
                model.Message = $"You won {Math.Round(result.Amount, 2)} {WebConstants.CurrencySymbols[user.Currency]}.";
            }
            else
            {
                model.Message = $"Bad luck. Try again!";
            }

            return this.PartialView("_GameSlotPartial", model);
        }

        [HttpGet]
        public async Task<IActionResult> SlotMachine(string size)
        {
            string gameName = "Slot Machine";
            int rows;
            int cols;

            switch (size)
            {
                case "4x3":
                    rows = 4;
                    cols = 3;
                    gameName = "Fruits Gone Wild";
                    break;
                case "5x5":
                    rows = 5;
                    cols = 5;
                    gameName = "Diamond Wild";
                    break;
                case "8x5":
                    rows = 8;
                    cols = 5;
                    gameName = "Classic 777";
                    break;
                default:
                    return this.RedirectToAction("Index");
            }

            var user = await userManager.GetUserAsync(HttpContext.User);
            var stringMatrix = game.GenerateMatrixWithItemNames(rows, cols);
            var convertedUserBalance = await this.userBalanceService.GetUserBalanceByIdAsync(user.Id);

            var model = new GameSlotViewModel()
            {
                GameName = gameName,
                Rows = rows,
                Cols = cols,
                Matrix = stringMatrix,
                Balance = Math.Round(convertedUserBalance, 2),
                Message = "Good luck!",
                Currency = user.Currency
            };

            return View(model);
        }
    }
}
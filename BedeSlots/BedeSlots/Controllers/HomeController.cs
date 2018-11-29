using BedeSlots.Data.Models;
using BedeSlots.Models;
using BedeSlots.Services.Data;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BedeSlots.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IExchangeRateApiCallService exchangeRateApiCallService;
        private readonly ICurrencyConverterService currencyConverterService;

        public HomeController(IExchangeRateApiCallService exchangeRateApiCallService, UserManager<User> userManager, ICurrencyConverterService currencyConverterService)
        {
            this.exchangeRateApiCallService = exchangeRateApiCallService;
            this.userManager = userManager;
            this.currencyConverterService = currencyConverterService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                decimal userBalance;
                if (user.Currency != Currency.USD)
                {
                    userBalance = await this.currencyConverterService.ConvertFromBaseToOther(user.Balance, user.Currency);
                }
                else
                {
                    userBalance = user.Balance;
                }

                ViewData["Balance"] = new UserBalanceViewModel() { Balance = userBalance };
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

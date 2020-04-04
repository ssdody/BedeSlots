using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class UserBalanceController : Controller
    {
        private readonly IUserBalanceService userBalanceService;
        private readonly UserManager<User> userManager;

        public UserBalanceController(IUserBalanceService userBalanceService, UserManager<User> userManager)
        {
            this.userBalanceService = userBalanceService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult BalanceViewComponent()
        {
            return ViewComponent("UserBalance");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> HasEnoughMoneyAsync(decimal amount)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var userBalance = await this.userBalanceService.GetUserBalanceByIdAsync(user.Id);

            return amount <= userBalance ? Json(true) : Json($"You don't have enough money!");
        }
    }
}
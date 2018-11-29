using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BedeSlots.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UserAdministrationController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;
        private readonly ITransactionService transactionService;

        public UserAdministrationController(IUserService userService, UserManager<User> userManager, ITransactionService transactionService)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.userService.GetAllUsersAsync();

            List<UserViewModel> userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var role = await this.userManager.GetRolesAsync(user);
                

                userViewModels.Add(new UserViewModel()
                {
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Balance = Math.Round(user.Balance, 2),
                    Email = user.Email,
                    Currency = user.Currency.ToString(),
                    Cards = user.Cards,
                    Transactions = user.Transactions,
                    Role = role.FirstOrDefault(),
                });
            }

            ////var usersViewModel = users.Select(u => new UserViewModel()
            ////{
            //Username = u.UserName,
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    Balance = u.Balance.ToString("0.00"),
            //    Email = u.Email,
            //    Currency = u.Currency.ToString(),
            //    Cards = u.Cards,
            //    Transactions = u.Transactions,
            ////});
            return View(userViewModels);
        }

    }
}
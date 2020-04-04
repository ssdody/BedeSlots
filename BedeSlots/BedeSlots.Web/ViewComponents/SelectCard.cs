using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.ViewComponents
{
    public class SelectCard : ViewComponent
    {
        private readonly ICardService cardService;
        private readonly UserManager<User> userManager;

        public SelectCard(ICardService cardService, UserManager<User> userManager)
        {
            this.cardService = cardService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);
            var cards = await this.cardService.GetUserCardsLastNumbersAsync(user.Id);
            var cardsSelectList = cards.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = "**** **** **** " + c.Number }).ToList();
            var selectCardViewModel = new SelectCardViewModel() { CardsList = cardsSelectList };

            return View("Default", selectCardViewModel);
        }
    }
}

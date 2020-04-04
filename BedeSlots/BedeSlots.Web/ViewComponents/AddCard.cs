using BedeSlots.Data.Models;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.ViewComponents
{
    public class AddCard : ViewComponent
    {
        public AddCard()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cardTypes = Enum.GetValues(typeof(CardType)).Cast<CardType>();
            var cardTypesSelectList = cardTypes.Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() }).ToList();

            var addCardVM = new AddCardViewModel() { CardTypes = cardTypesSelectList };

            return View("Default", addCardVM);
        }
    }
}

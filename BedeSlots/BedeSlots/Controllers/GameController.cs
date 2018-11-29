using BedeSlots.Games.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BedeSlots.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGame game;

        public GameController(IGame game)
        {
            this.game = game;
        }

        public IActionResult Index()
        {
            var matrix = game.GenerateMatrix(4, 3, null);
            var stringMatrix = game.GetCharMatrix(matrix);

            var model = new GameSlotViewModel()
            {
                Rows = 4,
                Cols = 3,
                Matrix = stringMatrix
            };

            return View(model);
        }

        public IActionResult Spin(decimal money)
        {
            var result = game.Spin(4, 3, money);

            var model = new GameSlotViewModel()
            {
                Rows = 4,
                Cols = 3,
                Matrix = result.Matrix,
                Money = result.Money
            };

            return this.PartialView("_GameSlotPartial", model);
        }
    }
}
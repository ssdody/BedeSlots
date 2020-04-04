using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models.GameViewModels
{
    public class GameStakeViewModel
    {
        [Required]
        [Range(1, WebConstants.MaxAmount, ErrorMessage = "The bet amount should be between 1 and 1 million.")]
        [RegularExpression(@"\d*\.?\d*", ErrorMessage = "The bet amount should be a number using dot for floating-point numbers.")]
        [Remote(action: "HasEnoughMoneyAsync", controller: "UserBalance", areaName: "")]
        public decimal Amount { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        public string StatusMessage { get; set; }
    }
}

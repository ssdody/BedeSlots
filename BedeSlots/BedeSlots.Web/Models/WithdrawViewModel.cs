using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class WithdrawViewModel
    {
        public WithdrawViewModel()
        {
        }

        [Range(1, WebConstants.MaxAmount, ErrorMessage = "The deposit amount should be between 1 and 1 million.")]
        [Remote(action: "HasEnoughMoneyAsync", controller: "UserBalance", areaName: "")]
        [RegularExpression(@"\d*\.?\d*", ErrorMessage = "The withdraw amount should be a number using dot for floating-point numbers.")]
        public decimal Amount { get; set; }

        [Required]
        [Range(1,100)]
        public int BankCardId { get; set; }

        [Required]
        public Currency Currency { get; set; }

        public string StatusMessage { get; set; }
    }
}

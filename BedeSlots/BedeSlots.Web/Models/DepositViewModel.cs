using BedeSlots.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class DepositViewModel
    {
        public DepositViewModel()
        {
        }

        [Required(ErrorMessage = "Please add a card before deposit!")]
        [Range(1, int.MaxValue)]
        public int BankCardId { get; set; }

        [Required(ErrorMessage = "Please enter a positive amount!")]
        [Range(1, WebConstants.MaxAmount, ErrorMessage ="The deposit amount should be between 1 and 1 million.")]
        [RegularExpression(@"\d*\.?\d*", ErrorMessage = "The deposit amount should be  a number using dot for floating-point numbers.")]
        [Display(Name = "Deposit")]
        public decimal DepositAmount { get; set; }

        public Currency Currency { get; set; }

        public string StatusMessage { get; set; }
    }
}


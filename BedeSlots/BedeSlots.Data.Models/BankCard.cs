
using BedeSlots.Common.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public class BankCard : Entity
    {
        [Required]
        [RegularExpression("^[0-9]{16}$", ErrorMessage = "The card number should be 16 digits.")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "The CVV number should be 3 digits.")]
        public string CvvNumber { get; set; }

        [Required]
        [ExpiryDate]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The cardholder name should be at least 3 symbols.")]
        [RegularExpression("^[A-Za-z]$", ErrorMessage = "The cardholer name should contains only letters.")]
        public string CardholerName { get; set; }

        [Required]
        public CardType Type { get; set; }

        public string UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}

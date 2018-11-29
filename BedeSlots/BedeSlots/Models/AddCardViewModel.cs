using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class AddCardViewModel
    {
        public AddCardViewModel()
        {
        }

        [Required]
        [RegularExpression("^[0-9]{16}$", ErrorMessage = "The card number should be 16 digits.")]
        [Display(Name = "Card number")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "The CVV number should be 3 digits.")]
        [Display(Name = "CVV")]
        public int Cvv { get; set; }

        [Required]
        [ExpiryDate]
        [Display(Name = "Expiry")]
        public DateTime Expiry { get; set; }

        [Required]
        [Display(Name = "Card type")]
        public CardType CardType { get; set; }

        public List<SelectListItem> CardTypes { get; set; }
    }
}

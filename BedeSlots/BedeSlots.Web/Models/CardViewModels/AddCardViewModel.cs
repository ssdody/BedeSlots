using BedeSlots.Common.CustomAttributes;
using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc;
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
        [Remote(action: "DoesCardExistInDatabase", controller: "Card", areaName: "")]
        [StringLength(19, MinimumLength =19, ErrorMessage ="The card number should be 16 digits.")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The cardholder name should be at least 3 symbols.")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "The cardholer name should contains only letters.")]
        [Display(Name = "Card Holder")]
        public string CardholderName { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "The CVV should be 3 or 4 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "The CVV should contains only digits.")]
        [Display(Name = "CVV")]
        public string Cvv { get; set; }

        [Required]
        [ExpiryDate(ErrorMessage = "Invalid expiry date!")]
        [Remote(action: "IsValidExpiryDate", controller: "Card", areaName: "")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Expiry Date")]
        public DateTime Expiry { get; set; }

        [Required]
        [Display(Name = "Card type")]
        public CardType CardType { get; set; }

        public List<SelectListItem> CardTypes { get; set; }
    }
}

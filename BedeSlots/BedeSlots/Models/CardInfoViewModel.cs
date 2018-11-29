using BedeSlots.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Models
{
    public class CardInfoViewModel
    {
        [Display(Name = "Card number")]
        public string CardNumber { get; set; }

        [Display(Name = "CVV")]
        public string Cvv { get; set; }

        [Display(Name = "Expiry date")]
        public string Expiry { get; set; }

        [Display(Name = "Card type")]
        public CardType CardType { get; set; }

        [Display(Name = "Currency")]
        public Currency Currency { get; set; }

        [Display(Name = "Owner")]
        public User Owner { get; set; }

        public CardInfoViewModel()
        {

        }
    }
}

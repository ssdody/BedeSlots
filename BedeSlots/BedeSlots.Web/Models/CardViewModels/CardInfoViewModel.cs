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
        public CardInfoViewModel()
        {
        }

        public int Id { get; set; }

        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Display(Name = "CVV")]
        public string Cvv { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-yy}")]
        public DateTime Expiry { get; set; }

        [Display(Name = "Card Type")]
        public string CardType { get; set; }

        [Display(Name = "Cardholder Name")]
        public string Cardholder { get; set; }
    }
}

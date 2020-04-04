using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class SelectCardViewModel
    {
        public SelectCardViewModel()
        {
        }

        [Display(Name = "Please select a card")]
        public List<SelectListItem> CardsList { get; set; }

        [Required(ErrorMessage = "Please add a card before deposit!")]
        [Range(1, int.MaxValue)]
        public int BankCardId { get; set; }
    }
}

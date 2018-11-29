using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Models
{
    public class DepositViewModel
    {
        [Required(ErrorMessage = "Please add a card before deposit!")]
        [Range(1, int.MaxValue)]
        public int BankCardId { get; set; }

        [Required(ErrorMessage = "Please enter a positive amount!")]
        [Range(1, double.MaxValue, ErrorMessage = "Deposit value must be positive number!")]
        [Display(Name = "Amount")]
        public decimal DepositAmount { get; set; }

        [Display(Name = "Select card")]
        public List<SelectListItem> BankCards { get; set; }
        public List<SelectListItem> CardTypes { get; set; }

        public DepositViewModel()
        {

        }
    }
}

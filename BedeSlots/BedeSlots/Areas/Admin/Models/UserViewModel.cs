using BedeSlots.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Areas.Admin.Models
{
    public class UserViewModel
    {
        [Display(Name ="Username")]
        public string Username { get; set; }
        [Display(Name ="Email")]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public IEnumerable<BankCard> Cards { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
        public string Role { get; set; }


        public UserViewModel()
        {

        }

    }
}

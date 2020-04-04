using BedeSlots.Data.Models;
using System;
using System.Collections.Generic;

namespace BedeSlots.Services.Data.Models.Users
{
    public class UserServiceModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthdate { get; set; }

        public string Email { get; set; }

        public Currency Currency { get; set; }

        public decimal Balance { get; set; }

        public ICollection<BankCard> Cards { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}

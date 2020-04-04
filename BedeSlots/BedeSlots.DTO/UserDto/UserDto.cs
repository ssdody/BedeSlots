using BedeSlots.Data.Models;
using System;
using System.Collections.Generic;

namespace BedeSlots.DTO
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime Birthdate { get; set; }

        public string Email { get; set; }

        public Currency Currency { get; set; }

        public decimal Balance { get; set; }

        public string Role { get; set; }

        public ICollection<BankCard> Cards { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}

using BedeSlots.Data.Models;
using System;

namespace BedeSlots.Web.Areas.Admin.Models.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthdate { get; set; }

        public string Email { get; set; }

        public Currency Currency { get; set; }

        public decimal Balance { get; set; }

        public string Role { get; set; }
    }
}

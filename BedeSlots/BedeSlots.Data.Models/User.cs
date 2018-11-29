using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MinLength(DataModelsConstants.UserNameMinLength)]
        [MaxLength(DataModelsConstants.UserNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(DataModelsConstants.UserNameMinLength)]
        [MaxLength(DataModelsConstants.UserNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Birthdate { get; set; }

        [Required]
        public Currency Currency { get; set; }

        public decimal Balance { get; set; }

        public ICollection<BankCard> Cards { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}

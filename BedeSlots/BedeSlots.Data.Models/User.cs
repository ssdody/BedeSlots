using BedeSlots.Data.Models.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public class User : IdentityUser, IAuditable, IDeletable
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

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
    }
}

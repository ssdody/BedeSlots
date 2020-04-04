using BedeSlots.Data.Models;
using System;

namespace BedeSlots.DTO.BankCardDto
{
    public class CardDetailsDto
    {
        public int Id { get; set; }

        public string LastFourDigit { get; set; }

        public string Cvv { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string CardholerName { get; set; }

        public CardType Type { get; set; }
    }
}

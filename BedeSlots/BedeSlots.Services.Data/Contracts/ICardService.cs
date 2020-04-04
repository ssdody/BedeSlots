using BedeSlots.Data.Models;
using BedeSlots.DTO.BankCardDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICardService
    {
        Task<ICollection<CardDetailsDto>> GetUserCardsAsync(string userId);

        Task<ICollection<CardNumberDto>> GetUserCardsLastNumbersAsync(string userId);

        Task<ICollection<CardNumberDto>> GetUserCardsAllNumbersAsync(string userId);

        Task<BankCard> AddCardAsync(string cardNumber, string cardholerName, string cvv, DateTime expiryDate, CardType cardType, string userId);

        bool CardExists(int bankCardId);

        Task<BankCard> DeleteCardAsync(int cardId);

        Task<CardDetailsDto> GetCardDetailsByIdAsync(int id);

        Task<CardNumberDto> GetCardNumberByIdAsync(int cardId);
    }
}
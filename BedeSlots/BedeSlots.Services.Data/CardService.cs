using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO.BankCardDto;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class CardService : ICardService
    {
        private readonly BedeSlotsDbContext context;
        private readonly UserManager<User> userManager;

        public CardService(BedeSlotsDbContext context, UserManager<User> userManager)
        {
            this.context = context ?? throw new ServiceException(nameof(context));
            this.userManager = userManager ?? throw new ServiceException(nameof(userManager));
        }

        public async Task<ICollection<CardDetailsDto>> GetUserCardsAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("userId is null!");
            }

            var exists = await this.context.Users.AnyAsync(u => u.Id == userId && u.IsDeleted == false);

            if (!exists)
            {
                throw new ServiceException($"User with id:{userId} doesn't exist!");
            }

            var cards = await this.context.BankCards
                .Where(c => c.UserId == userId && c.IsDeleted == false)
                .Select(c => new CardDetailsDto
                {
                    LastFourDigit = c.Number.Substring(12),
                    CardholerName = c.CardholerName,
                    Cvv = c.CvvNumber,
                    ExpiryDate = c.ExpiryDate,
                    Type = c.Type
                })
                .ToListAsync();

            return cards;
        }

        public async Task<ICollection<CardNumberDto>> GetUserCardsLastNumbersAsync(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId is null!");
            }

            var exists = await this.context.Users.AnyAsync(u => u.Id == userId);

            if (!exists)
            {
                throw new ArgumentException($"User with id:{userId} doesn't exist!");
            }

            var cardsNumbers = await this.context.BankCards
                .Where(c => c.UserId == userId && c.IsDeleted == false)
                .Select(c => new CardNumberDto
                {
                    Id = c.Id,
                    Number = c.Number.Substring(12)
                })
                .ToListAsync();

            return cardsNumbers;
        }

        public async Task<ICollection<CardNumberDto>> GetUserCardsAllNumbersAsync(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId is null!");
            }

            var exists = await this.context.Users.AnyAsync(u => u.Id == userId);

            if (!exists)
            {
                throw new ArgumentException($"User with id:{userId} doesn't exist!");
            }

            var cardsNumbers = await this.context.BankCards
                .Where(c => c.UserId == userId && c.IsDeleted == false)
                .Select(c => new CardNumberDto
                {
                    Id = c.Id,
                    Number = c.Number
                })
                .ToListAsync();

            return cardsNumbers;
        }

        public async Task<BankCard> AddCardAsync(string cardNumber, string cardholerName, string cvv, DateTime expiryDate, CardType cardType, string userId)
        {
            var cardNumberWithoutSpaces = cardNumber.Replace(" ", string.Empty);

            var card = new BankCard()
            {
                Number = cardNumberWithoutSpaces,
                CardholerName = cardholerName,
                CvvNumber = cvv,
                ExpiryDate = expiryDate,
                Type = cardType,
                UserId = userId,
            };

            await this.context.BankCards.AddAsync(card);
            await this.context.SaveChangesAsync();

            return card;
        }

        public async Task<BankCard> DeleteCardAsync(int cardId)
        {
            var card = await this.GetCardByIdAsync(cardId);
            card.IsDeleted = true;
            await this.context.SaveChangesAsync();

            return card;
        }

        public async Task<BankCard> GetCardByIdAsync(int id)
        {
            var card = await this.context.BankCards
                                 .Where(c => c.IsDeleted == false)
                                 .Include(c => c.User)
                                 .FirstOrDefaultAsync(c => c.Id == id);

            if (card == null)
            {
                throw new ServiceException($"There is no card with id {id}");
            }

            return card;
        }

        public bool CardExists(int bankCardId)
        {
            return this.context.BankCards.Any(c => c.Id == bankCardId);
        }

        public async Task<CardDetailsDto> GetCardDetailsByIdAsync(int cardId)
        {
            var card = await this.context.BankCards
                .Where(c => c.IsDeleted == false)
                .Select(c => new CardDetailsDto()
                {
                    Id = c.Id,
                    CardholerName = c.CardholerName,
                    Cvv = c.CvvNumber,
                    ExpiryDate = c.ExpiryDate,
                    LastFourDigit = c.Number.Substring(12),
                    Type = c.Type
                })
                .FirstOrDefaultAsync(c => c.Id == cardId);

            if (card == null)
            {
                throw new ServiceException($"There is no card with id {cardId}");
            }

            return card;
        }

        public async Task<CardNumberDto> GetCardNumberByIdAsync(int cardId)
        {
            var card = await this.context.BankCards
                .Where(c => c.IsDeleted == false)
                .Select(c => new CardNumberDto()
                {
                    Id = c.Id,
                    Number = c.Number.Substring(12)
                })
                .FirstOrDefaultAsync(c => c.Id == cardId);

            if (card == null)
            {
                throw new ServiceException($"There is no card with id {cardId}");
            }

            return card;
        }
    }
}

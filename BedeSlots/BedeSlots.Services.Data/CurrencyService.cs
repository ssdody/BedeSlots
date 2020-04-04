using System;
using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class CurrencyService : ICurrencyService
    {
        private readonly BedeSlotsDbContext context;

        public CurrencyService(BedeSlotsDbContext context)
        {
            this.context = context ?? throw new ServiceException(nameof(context));
        }

        public async Task<Currency> GetUserCurrencyAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            if (!await this.context.Users.AnyAsync(u => u.Id == userId))
            {
                throw new ServiceException("User not exist in database!");
            }

            var currency = await this.context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Currency)
                .FirstOrDefaultAsync();

            if (currency == 0)
            {
                throw new ServiceException("User currency not set!");
            }

            return currency;
        }

        public ICollection<Currency> GetAllCurrencies()
        {
            return Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
        }
    }
}

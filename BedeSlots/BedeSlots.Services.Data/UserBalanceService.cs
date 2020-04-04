using BedeSlots.Common;
using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class UserBalanceService : IUserBalanceService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ICurrencyConverterService currencyConverterService;

        public UserBalanceService(BedeSlotsDbContext context, ICurrencyConverterService currencyConverterService)
        {
            this.context = context ?? throw new ServiceException(nameof(context));
            this.currencyConverterService = currencyConverterService ?? throw new ServiceException(nameof(currencyConverterService));
        }

        public async Task<decimal> DepositMoneyAsync(decimal amount, string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            if (amount <= 0)
            {
                throw new ServiceException("Amount must be posititive number!");
            }

            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new ServiceException("User not exist in database!");

            if (user.Currency != ServicesConstants.BaseCurrency)
            {
                amount = await this.currencyConverterService.ConvertToBaseCurrencyAsync(amount, user.Currency);
            }

            user.Balance += Math.Round(amount, 2);

            this.context.Update(user);
            await this.context.SaveChangesAsync();

            return amount;
        }

        public async Task<decimal> ReduceMoneyAsync(decimal amount, string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            if (amount <= 0)
            {
                throw new ServiceException("Amount must be posititive!");
            }

            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new ServiceException("User not exist!");

            if (user.Currency != ServicesConstants.BaseCurrency)
            {
                amount = await this.currencyConverterService.ConvertToBaseCurrencyAsync(amount, user.Currency);
            }

            if (user.Balance >= amount)
            {
                user.Balance -= Math.Round(amount, 2);
            }
            else
            {
                throw new ServiceException("Not enough money!");
            }

            this.context.Update(user);
            await this.context.SaveChangesAsync();
            return amount;
        }

        public async Task<decimal> GetUserBalanceByIdAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            var user = await this.context.Users
                            .Where(u => u.Id == userId)
                            .Select(u => new
                            {
                                u.Balance,
                                u.Currency
                            })
                            .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ServiceException($"User with Id:{userId} not exist!");
            }

            decimal balance = user.Balance;

            if (user.Currency != ServicesConstants.BaseCurrency)
            {
                balance = await this.currencyConverterService.ConvertFromBaseToOtherAsync(balance, user.Currency);
            }

            return balance;
        }

        public async Task<decimal> GetUserBalanceByIdInBaseCurrencyAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            if (!await context.Users.AnyAsync(u => u.Id == userId))
            {
                throw new ServiceException($"User with Id:{userId} not exist!");
            }

            var balance = await this.context.Users
                            .Where(u => u.Id == userId)
                            .Select(u => u.Balance)
                            .FirstOrDefaultAsync();

            return balance;
        }
    }
}

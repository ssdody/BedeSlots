using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class DepositService : IDepositService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ICurrencyConverterService currencyConverterService;

        public DepositService(BedeSlotsDbContext context, ICurrencyConverterService currencyConverterService)
        {
            this.context = context;
            this.currencyConverterService = currencyConverterService;
        }

        public async Task<Transaction> DepositAsync(Transaction transaction)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == transaction.UserId);
            decimal amount = transaction.Amount;

            if (user.Currency != Currency.USD)
            {
                amount = await this.currencyConverterService.ConvertToBaseCurrency(transaction.Amount, user.Currency);
            }

            user.Balance += amount;

            this.context.Update(user);
            await this.context.SaveChangesAsync();
            return transaction;
        }
    }
}

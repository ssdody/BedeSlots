using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IUserBalanceService
    {
        Task<decimal> DepositMoneyAsync(decimal amount, string userId);

        Task<decimal> ReduceMoneyAsync(decimal amount, string userId);

        Task<decimal> GetUserBalanceByIdAsync(string userId);

        Task<decimal> GetUserBalanceByIdInBaseCurrencyAsync(string userId);
    }
}
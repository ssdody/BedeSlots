using BedeSlots.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserById(string id);
        Task<decimal> GetUserBalanceById(string userId);
        Task<IList<User>> GetAllUsersAsync();
        Task<string> GetUserRole(User user);
        Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string id);
    }
}
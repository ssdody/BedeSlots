using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class UserService : IUserService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ITransactionService transactionService;

        public UserService(BedeSlotsDbContext bedeSlotsDbContext, ITransactionService transactionService)
        {
            this.context = bedeSlotsDbContext;
            this.transactionService = transactionService;
        }

        public async Task<decimal> GetUserBalanceById(string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user.Balance;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<IList<User>> GetAllUsersAsync()
        {
            var users = await this.context.Users.Include(u=>u.Transactions).ToListAsync();

            return users;
        }

        public async Task<string> GetUserRole(User user)
        {
            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == user.Id);

            return role.ToString();
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string id)
        {
            var transactions = await this.context.Transactions.Where(t => t.UserId == id).ToListAsync();

            return transactions;
        }

    }
}

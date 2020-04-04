using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.TransactionService
{
    [TestClass]
    public class GetUserTransactions_Should
    {
        [TestMethod]
        public async Task ReturnCollectionOfUserTransactions_WhenValidUserIdIsPassed()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
   .UseInMemoryDatabase(databaseName: "ReturnCollectionOfUserTransactions_WhenValidUserIdIsPassed")
   .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User() { Email = "test" };
            var otherUser = new User();

            var transaction = new Transaction()
            {
                User = user,
                UserId = user.Id,
                Type = TransactionType.Deposit
            };

            var secondTransaction = new Transaction()
            {
                User = user,
                UserId = user.Id,
                Type = TransactionType.Deposit
            };

            var thirdTransaction = new Transaction()
            {
                User = otherUser,
                UserId = otherUser.Id,
                Type = TransactionType.Deposit
            };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                await bedeSlotsContext.Users.AddAsync(user);
                await bedeSlotsContext.Users.AddAsync(otherUser);

                await bedeSlotsContext.Transactions.AddAsync(transaction);
                await bedeSlotsContext.Transactions.AddAsync(secondTransaction);
                await bedeSlotsContext.Transactions.AddAsync(thirdTransaction);
                await bedeSlotsContext.SaveChangesAsync();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);
                var result = sut.GetUserTransactionsAsync(user.Id);

                Assert.IsTrue(await result.CountAsync() == 2);
            }
        }
    }
}

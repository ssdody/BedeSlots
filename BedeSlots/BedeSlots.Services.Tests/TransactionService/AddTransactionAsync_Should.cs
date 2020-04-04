using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.TransactionService
{
    [TestClass]
    public class AddTransactionAsync_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task AddTransactionCorrectly_WhenValidParamentersArePassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "AddTransactionCorrectly_WhenValidParamentersArePassed")
    .UseInternalServiceProvider(serviceProvider).Options;

            var validTransactionType = TransactionType.Deposit;

            var user = new User();

            string description = "test";

            int validAmount = 100;

            var baseCurrency = Currency.USD;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);
                currencyConverterMock.Setup((x) => x.ConvertToBaseCurrencyAsync(10, Currency.USD)).ReturnsAsync(10);

                await sut.AddTransactionAsync(validTransactionType, user.Id, description, validAmount, baseCurrency);
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                Assert.IsTrue(await bedeSlotsContext.Transactions.CountAsync() == 1);
                Assert.IsTrue(await bedeSlotsContext.Transactions.AnyAsync(x => x.Description == description));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNegativeAmountIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNegativeAmountIsPassed")
    .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();

            var validTransactionType = TransactionType.Deposit;

            var baseCurrency = Currency.USD;

            string description = "test";

            int negativeAmount = -100;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.AddTransactionAsync(validTransactionType, user.Id, description, negativeAmount, baseCurrency));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenUserIdParameterIsNull()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenUserIdParameterIsNull")
    .UseInternalServiceProvider(serviceProvider).Options;

            var validTransactionType = TransactionType.Deposit;

            string description = "test";

            int validAmount = 100;

            var baseCurrency = Currency.USD;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.AddTransactionAsync(validTransactionType, null, description, validAmount, baseCurrency));
            }
        }
    }
}

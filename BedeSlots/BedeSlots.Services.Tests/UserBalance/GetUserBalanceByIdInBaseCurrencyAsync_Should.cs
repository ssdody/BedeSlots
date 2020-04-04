using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.UserBalance
{
    [TestClass]
    public class GetUserBalanceByIdInBaseCurrencyAsync_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ThrowServiceException_WhenNullParameterIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNullParameterIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.GetUserBalanceByIdInBaseCurrencyAsync(null));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenUnexistingUserIdIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenUnexistingUserIdIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                var notExistingUserId = "not existing id";

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () =>
                await sut.GetUserBalanceByIdInBaseCurrencyAsync(notExistingUserId));
            }
        }

        [TestMethod]
        public async Task ReturnUserBalance_WhenValidUserIdIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "ReturnUserBalance_WhenValidUserIdIsPassed")
    .UseInternalServiceProvider(serviceProvider).Options;

            var expectedBalance = 100;

            var user = new User() { Balance = expectedBalance, Currency = Currency.USD };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                var result = await sut.GetUserBalanceByIdInBaseCurrencyAsync(user.Id);

                Assert.IsTrue(result == expectedBalance);
            }
        }
    }
}

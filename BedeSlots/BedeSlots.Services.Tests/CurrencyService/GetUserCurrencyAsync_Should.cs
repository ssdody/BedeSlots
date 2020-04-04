using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.CurrencyService
{
    [TestClass]
    public class GetUserCurrencyAsync_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReturnUserCurrency_WhenValidUserIdIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
                .UseInMemoryDatabase(databaseName: "ReturnUserCurrency_WhenValidUserIdIsPassed")
                .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User()
            {
                Currency = Currency.BGN
            };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                await bedeSlotsContext.Users.AddAsync(user);
                await bedeSlotsContext.SaveChangesAsync();

            }
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.CurrencyService(bedeSlotsContext);
                var result = await sut.GetUserCurrencyAsync(user.Id);

                Assert.IsTrue(result == user.Currency);
                Assert.IsInstanceOfType(result, typeof(Currency));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenUserCurrencyIsNull()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenUserCurrencyIsNull")
                .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                await bedeSlotsContext.Users.AddAsync(user);
                await bedeSlotsContext.SaveChangesAsync();

            }
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.CurrencyService(bedeSlotsContext);
                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.GetUserCurrencyAsync(user.Id));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenThereAreNoUsersInDatabase()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
                .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenThereAreNoUsersInDatabase")
                .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.CurrencyService(bedeSlotsContext);
                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.GetUserCurrencyAsync("test"));
            }
        }

        [TestMethod]
        public void ThrowServiceException_WhenNullUserIdIsPassed()
        {
            Assert.ThrowsException<ServiceException>(() => new Data.CurrencyService(null));
        }
    }
}

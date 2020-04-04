using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.UserBalance
{
    [TestClass]
    public class DepositMoneyAsync_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task IncreaseUserBalance_WhenValidParametersArePassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "IncreaseUserBalance_WhenValidParametersArePassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var tenDollars = 10;

            var user = new User() { Currency = Currency.USD };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);
                var result = await sut.DepositMoneyAsync(tenDollars, user.Id);

                Assert.IsTrue(result == tenDollars);
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNullUserIdParameterIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNullUserIdParameterIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                decimal validNumber = 1;

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.DepositMoneyAsync(validNumber, null));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNegativeAmountParameterIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNegativeAmountParameterIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                decimal invalidNumber = -10;

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.DepositMoneyAsync(invalidNumber, "valid id"));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenUserNotExistInDatabase()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenUserNotExistInDatabase")
     .UseInternalServiceProvider(serviceProvider).Options;

            var tenDollars = 10;

            var user = new User() { Balance = tenDollars + tenDollars, Currency = Currency.USD };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.DepositMoneyAsync(tenDollars, user.Id));
            }
        }
    }
}

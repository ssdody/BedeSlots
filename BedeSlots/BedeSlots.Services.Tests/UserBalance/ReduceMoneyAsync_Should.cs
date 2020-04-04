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
    public class ReduceMoneyAsync_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReduceUserBalance_WhenValidParametersArePassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReduceUserBalance_WhenValidParametersArePassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var tenDollars = 10;

            var user = new User() { Balance = tenDollars + tenDollars, Currency = Currency.USD };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                var result = await sut.ReduceMoneyAsync(tenDollars, user.Id);

                Assert.IsTrue(result == tenDollars);
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

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.ReduceMoneyAsync(tenDollars, user.Id));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenAmountParameterIsBiggerThanUserBalance()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReduceUserBalance_WhenValidParametersArePassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var tenDollars = 10;

            var amountBiggerThanUserBalance = 5000;

            var user = new User() { Balance = tenDollars, Currency = Currency.USD };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.ReduceMoneyAsync(amountBiggerThanUserBalance, user.Id));
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

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.ReduceMoneyAsync(validNumber, null));
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

                string validId = "pseudo valid id";

                decimal invalidNumber = -10;

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () =>
                await sut.ReduceMoneyAsync(invalidNumber, validId));
            }
        }
    }
}

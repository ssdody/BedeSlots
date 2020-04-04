using BedeSlots.Data;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.Services.Tests.UserBalance
{
    [TestClass]
    public class Constructor_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public void CreateNewInstance_WhenValidParametersArePassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "CreateNewInstance_WhenValidParametersArePassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();

                var sut = new Data.UserBalanceService(bedeSlotsContext, currencyConverterMock.Object);

                Assert.IsInstanceOfType(sut, typeof(Data.UserBalanceService));
            }
        }

        [TestMethod]
        public void ThrowServiceExcpetion_WhenNullCurrencyConverterParameterIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceExcpetion_WhenNullCurrencyConverterParameterIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                Assert.ThrowsException<ServiceException>(() => new Data.UserBalanceService(bedeSlotsContext, null));
            }
        }

        [TestMethod]
        public void ThrowServiceExcpetion_WhenNullContextParameterIsPassed()
        {
            var currencyConverterMock = new Mock<ICurrencyConverterService>();

            Assert.ThrowsException<ServiceException>(() => new Data.UserBalanceService(null, currencyConverterMock.Object));
        }

    }
}
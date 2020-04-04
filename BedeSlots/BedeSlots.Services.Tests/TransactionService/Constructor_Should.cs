using BedeSlots.Data;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BedeSlots.Services.Tests.TransactionService
{
    [TestClass]
    public class Constructor_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public void ThrowServiceException_WhenNullContextParameterIsPassed()
        {
            var currencyConverterMock = new Mock<ICurrencyConverterService>();

            Assert.ThrowsException<ServiceException>(() => new Data.TransactionService(null, currencyConverterMock.Object));
        }

        [TestMethod]
        public void ThrowServiceException_WhenNullCurrencyConverterParameterIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNullCurrencyConverterParameterIsPassed")
    .UseInternalServiceProvider(serviceProvider).Options;
            
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                Assert.ThrowsException<ServiceException>(() => new Data.TransactionService(bedeSlotsContext, null));
            }
        }

        [TestMethod]
        public void InitializeCorrectly_WhenValidParametersArePassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "InitializeCorrectly_WhenValidParametersArePassed")
    .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);

                Assert.IsInstanceOfType(sut, typeof(Data.TransactionService));
            }
        }
    }
}
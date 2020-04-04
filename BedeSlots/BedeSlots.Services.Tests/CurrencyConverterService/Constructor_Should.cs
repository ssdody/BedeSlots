using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BedeSlots.Services.Tests.CurrencyConverterService
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ThrowServiceException_WhenNullExchangeRateApiCallerIsPassed()
        {
            Assert.ThrowsException<ServiceException>(() => new Data.CurrencyConverterService(null));
        }

        [TestMethod]
        public void InitializeCorrectly_WhenValidExchangeRateApiCallerIsPassed()
        {
            var eracMock = new Mock<IExchangeRateApiService>();
            var sut = new Data.CurrencyConverterService(eracMock.Object);

            Assert.IsInstanceOfType(sut, typeof(Data.CurrencyConverterService));
        }
    }
}

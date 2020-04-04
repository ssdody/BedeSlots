using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using BedeSlots.Services.External.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.CurrencyConverterService
{
    [TestClass]
    public class ConvertFromBaseToOtherAsync_Should
    {
        [TestMethod]
        public async Task ReturnAmountConvertedFromBaseToOther_WhenValidParametersArePassed()
        {
            decimal inputVal = 10;
            decimal exchangeRateApiCallerMockReturnVal = 5;
            decimal expectedVal = inputVal * exchangeRateApiCallerMockReturnVal;

            var exchangeRateApiCallerMock = new Mock<IExchangeRateApiService>();
            exchangeRateApiCallerMock.Setup(e => e.GetRateAsync(It.IsAny<Currency>())).ReturnsAsync(exchangeRateApiCallerMockReturnVal);

            var sut = new Data.CurrencyConverterService(exchangeRateApiCallerMock.Object);

            var result = await sut.ConvertFromBaseToOtherAsync(inputVal, Currency.USD);

            Assert.IsTrue(result == expectedVal);
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNegativeAmountIsPassed()
        {
            decimal negativeNumber = -44;

            var exchangeRateApiCallerMock = new Mock<IExchangeRateApiService>();

            var sut = new Data.CurrencyConverterService(exchangeRateApiCallerMock.Object);

            await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.ConvertFromBaseToOtherAsync(negativeNumber, Currency.BGN));
        }
    }
}

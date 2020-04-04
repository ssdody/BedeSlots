using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data;
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

namespace BedeSlots.Services.Tests.CurrencyConverterService
{
    [TestClass]
    public class ConvertToBaseCurrency_Should
    {
        [TestMethod]
        public async Task ReturnNumberConvertedToBaseCurrency_WhenValidParametersArePassed()
        {
            decimal inputVal = 10;
            decimal returnVal = 5;
            decimal expectedVal = inputVal * (1 / returnVal);

            var exchangeRateApiCallerMock = new Mock<IExchangeRateApiService>();
            exchangeRateApiCallerMock.Setup(e => e.GetRateAsync(It.IsAny<Currency>())).ReturnsAsync(returnVal);

            var sut = new Data.CurrencyConverterService(exchangeRateApiCallerMock.Object);

            var result = await sut.ConvertToBaseCurrencyAsync(inputVal, Currency.BGN);

            Assert.IsTrue(result == expectedVal);
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNegativeAmountIsPassed()
        {
            decimal negativeNumber = -44;

            var exchangeRateApiCallerMock = new Mock<IExchangeRateApiService>();

            var sut = new Data.CurrencyConverterService(exchangeRateApiCallerMock.Object);

            await Assert.ThrowsExceptionAsync<ServiceException>(async () =>
            await sut.ConvertToBaseCurrencyAsync(negativeNumber, Currency.BGN));
        }
    }
}

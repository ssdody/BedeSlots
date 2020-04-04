using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace BedeSlots.Services.External.Tests
{
    [TestClass]
    public class GetCurrenciesRatesAsync_Should
    {
        [TestMethod]
        public async Task ReturnUnavalibleMessage_WhenInvalidParametersPassed()
        {
            // Arrange
            var sut = new ExchangeRatesApiCaller();

            //Act 
            var result = await sut.GetCurrenciesRatesAsync("https://apiurl.io", "parameters");

            StringAssert.Contains(result, "not available");
        }

        [TestMethod]
        public async Task ThrowArgumentNullException_WhenNullParametersPassed()
        {
            // Arrange
            var sut = new ExchangeRatesApiCaller();

            //Act && Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sut.GetCurrenciesRatesAsync(null, null));
        }
    }
}

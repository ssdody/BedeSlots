using BedeSlots.Data;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BedeSlots.Services.Tests.CurrencyService
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ThrowServiceException_WhenNullContextParameterIsPassed()
        {
            Assert.ThrowsException<ServiceException>(() => new Data.CurrencyService(null));
        }

        [TestMethod]
        public void InitializeCorrectly_WhenValidContextIsPassed()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
                     .UseInMemoryDatabase(databaseName: "InitializeCorrectly_WhenValidContextIsPassed")
                     .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.CurrencyService(bedeSlotsContext);
                Assert.IsInstanceOfType(sut, typeof(Data.CurrencyService));
            }
        }
    }
}

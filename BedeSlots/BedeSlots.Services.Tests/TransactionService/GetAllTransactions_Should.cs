using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.TransactionService
{
    [TestClass]
    public class GetAllTransactions_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReturnCollectionOfAllTransaction_WhenValidParametersArePassed()
        {

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "ReturnCollectionOfAllTransaction_WhenValidParametersArePassed")
    .UseInternalServiceProvider(serviceProvider).Options;
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Transactions.Add(new Transaction() { User = new User() { Email = "test" } });
                bedeSlotsContext.SaveChanges();
            }
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);
                var result = sut.GetAllTransactions();

                Assert.IsTrue(await result.CountAsync() == 1);
            }
        }

        [TestMethod]
        public async Task ReturnEmptyCollectionOfAllTransaction_WhenValidParametersArePassed()
        {

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "ReturnEmptyCollectionOfAllTransaction_WhenValidParametersArePassed")
    .UseInternalServiceProvider(serviceProvider).Options;
            
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);
                var result = sut.GetAllTransactions();

                Assert.IsTrue(await result.CountAsync() == 0);
            }
        }
    }
}


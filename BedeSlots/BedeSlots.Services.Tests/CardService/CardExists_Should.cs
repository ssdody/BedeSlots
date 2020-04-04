using BedeSlots.Data;
using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class CardExists_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReturnTrue_WhenCardExistsInDatabase()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnTrue_WhenCardExistsInDatabase")
     .UseInternalServiceProvider(serviceProvider)
     .Options;

            var user = new User();
            var card = new BankCard();

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                await bedeSlotsContext.BankCards.AddAsync(card);
                await bedeSlotsContext.SaveChangesAsync();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.CardService(bedeSlotsContext, userManager);

                Assert.IsTrue(sut.CardExists(card.Id));
            }

        }

        [TestMethod]
        public void ReturnFalse_WhenCardExistsInDatabase()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var notExistingCardId = 1;

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnFalse_WhenCardExistsInDatabase")
     .UseInternalServiceProvider(serviceProvider)
     .Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.CardService(bedeSlotsContext, userManager);

                Assert.IsFalse(sut.CardExists(notExistingCardId));
            }

        }
    }
}


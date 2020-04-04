using BedeSlots.Data;
using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class DeleteCardAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task SetCardsIsDeletedPropertyToTrue_WhenValidIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "SetCardsIsDeletedPropertyToTrue_WhenValidIsPassed")
     .UseInternalServiceProvider(serviceProvider)
     .Options;

            var user = new User();
            var firstCard = new BankCard()
            {
                UserId = user.Id,
                User = user,
                CvvNumber = "123",
                Number = "1616161616161616",
                Type = CardType.Visa,
                CreatedOn = DateTime.Now
            };
            var secondCard = new BankCard()
            {
                UserId = user.Id,
                CvvNumber = "123",
                Number = "6161616161616161",
                Type = CardType.Visa,
                CreatedOn = DateTime.Now
            };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                await bedeSlotsContext.BankCards.AddAsync(firstCard);
                await bedeSlotsContext.SaveChangesAsync();

                var sut = new Data.CardService(bedeSlotsContext, userManager);

                await sut.DeleteCardAsync(firstCard.Id);
            }
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                Assert.IsTrue(await bedeSlotsContext.BankCards.AnyAsync(x => x.IsDeleted == true));
            }
        }
    }
}
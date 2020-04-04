using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO.BankCardDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class GetUserCardsAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ThrowServiceException_WhenNullParameterIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNullParameterIsPassed").UseInternalServiceProvider(serviceProvider)
     .Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var cardService = new Data.CardService(bedeSlotsContext, userManager);
                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await cardService.GetUserCardsAsync(null));
            }
        }
        [TestMethod]
        public async Task ThrowServiceException_WhenUnexistingUserIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenUnexistingUserIdIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            string notExistingId = "not existing id";

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var cardService = new Data.CardService(bedeSlotsContext, userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () =>
                await cardService.GetUserCardsAsync(notExistingId));
            }
        }
        [TestMethod]
        public async Task ReturnUserCards_WhenExistingUserIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnUserCards_WhenExistingUserIdIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();
            var card = new BankCard()
            {
                UserId = user.Id,
                User = user,
                CvvNumber = "123",
                Number = "1616161616161616",
                Type = CardType.Visa,
                CreatedOn = DateTime.Now
            };

            Data.CardService cardService;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.BankCards.Add(card);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                cardService = new Data.CardService(bedeSlotsContext, userManager);
                var result = await cardService.GetUserCardsAsync(user.Id);

                Assert.IsTrue(result.Count == 1);
            }
        }
    }
}

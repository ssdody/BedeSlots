using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.UserService
{
    [TestClass]
    public class GetUserByIdAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ThrowServiceException_WhenNotExistingIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
                        .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNotExistingIdIsPassed")
                        .UseInternalServiceProvider(serviceProvider).Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                     userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.GetUserByIdAsync("test"));
            }
        }

        [TestMethod]
        public async Task ReturnUser_WhenValidIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnUser_WhenValidIdIsPassed")
     .UseInternalServiceProvider(serviceProvider)
     .Options;

            var user = new User()
            {
                UserName = "test"
            };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                     userManager);

                var result = await sut.GetUserByIdAsync(user.Id);

                Assert.IsTrue(result != null);
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNullParameterIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
                        .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNullParameterIsPassed")
                        .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                     userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.GetUserByIdAsync(null));
            }
        }
    }
}

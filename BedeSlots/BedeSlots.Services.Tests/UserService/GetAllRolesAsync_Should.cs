using BedeSlots.Data;
using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.UserService
{
    [TestClass]
    public class GetAllRolesAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReturnCollectionOfAllRoles_WhenInvoked()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnCollectionOfAllRoles_WhenInvoked")
     .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var role = new IdentityRole("User");

                bedeSlotsContext.Roles.Add(role);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                    userManager);

                var result = await sut.GetAllRolesAsync();

                Assert.IsInstanceOfType(result, typeof(ICollection<IdentityRole>));
                Assert.IsTrue(result.Count == 1);
            }
        }

        [TestMethod]
        public async Task ReturnEmptyCollectionOfAllRoles_WhenInvoked()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnEmptyCollectionOfAllRoles_WhenInvoked")
     .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                    userManager);

                var result = await sut.GetAllRolesAsync();

                Assert.IsInstanceOfType(result, typeof(ICollection<IdentityRole>));
                Assert.IsTrue(result.Count == 0);
            }
        }

    }
}

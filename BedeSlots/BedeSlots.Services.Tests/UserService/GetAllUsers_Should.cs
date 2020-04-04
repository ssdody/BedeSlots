using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.UserService
{
    [TestClass]
    public class GetAllUsers_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public void ReturnCollectionOfAllUsers_WhenInvoked()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnCollectionOfAllUsers_WhenInvoked")
     .UseInternalServiceProvider(serviceProvider).Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            var user = new User() { };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Roles.Add(new IdentityRole("User"));
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.SaveChanges();

                var roleId = bedeSlotsContext.Roles.First().Id;

                var userRole = new IdentityUserRole<string>() { UserId = user.Id, RoleId = roleId };

                bedeSlotsContext.UserRoles.Add(userRole);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                    userManager);

                Assert.IsTrue(sut.GetAllUsers().Count() == 1);
            }
        }

        [TestMethod]
        public void ReturnEmptyCollectionUsers_WhenInvoked()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnEmptyCollectionUsers_WhenInvoked")
     .UseInternalServiceProvider(serviceProvider).Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Roles.Add(new IdentityRole("User"));
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                    userManager);

                Assert.IsTrue(sut.GetAllUsers().Count() == 0);
            }
        }
    }
}

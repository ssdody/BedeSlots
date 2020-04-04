using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
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
    public class EditUserRoleAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task SuccessfullyChangeUserCurrentRole_WhenValidParametersArePassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "SuccessfullyChangeUserCurrentRole_WhenValidParametersArePassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();

            var role = new IdentityRole("User");

            var newRole = new IdentityRole("newRole");

            IdentityRole result;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Roles.Add(role);
                bedeSlotsContext.Roles.Add(newRole);
                bedeSlotsContext.Users.Add(user);

                var userRole = new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id };

                bedeSlotsContext.UserRoles.Add(userRole);
                bedeSlotsContext.SaveChanges();

                var sut = new Data.UserService(bedeSlotsContext, userManager);

                result = await sut.EditUserRoleAsync(user.Id, newRole.Id);
            }

            Assert.IsTrue(result.Name == newRole.Name);
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenInvalidNewRoleIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenInvalidNewRoleIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();

            var role = new IdentityRole("User");

            var newRole = new IdentityRole("newRole");

            var notExistingRoleId = "not existing role";

            IdentityUserRole<string> userRole;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Roles.Add(role);
                bedeSlotsContext.Roles.Add(newRole);
                bedeSlotsContext.Users.Add(user);

                userRole = new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id };

                bedeSlotsContext.UserRoles.Add(userRole);
                bedeSlotsContext.SaveChanges();

            }
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext, userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () =>
                await sut.EditUserRoleAsync(user.Id, notExistingRoleId));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenInvalidUserIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenInvalidUserIdIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();

            var role = new IdentityRole("User");

            var newRole = new IdentityRole("newRole");

            var notExistingUserId = "not existing role";

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Roles.Add(role);
                bedeSlotsContext.Roles.Add(newRole);
                bedeSlotsContext.Users.Add(user);

                var userRole = new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id };

                bedeSlotsContext.UserRoles.Add(userRole);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext, userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () =>
                await sut.EditUserRoleAsync(notExistingUserId, newRole.Id));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenIvalidUserIdNotExistInDatabase()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenIvalidUserIdNotExistInDatabase")
     .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();

            var role = new IdentityRole("User");

            var newRole = new IdentityRole("newRole");

            var notExistingUserId = "not existing role";

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Roles.Add(role);
                bedeSlotsContext.Roles.Add(newRole);
                bedeSlotsContext.Users.Add(user);

                var userRole = new IdentityUserRole<string>() { UserId = notExistingUserId, RoleId = role.Id };

                bedeSlotsContext.UserRoles.Add(userRole);
                bedeSlotsContext.SaveChanges();

                var sut = new Data.UserService(bedeSlotsContext, userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () =>
                await sut.EditUserRoleAsync(notExistingUserId, newRole.Id));
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNullUserIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "SuccessfullyChangeUserCurrentRole_WhenValidParametersArePassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var newRole = new IdentityRole("newRole");

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                                               userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.EditUserRoleAsync(null, newRole.Id));

            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenInvalidNewRoleIdIsPassed()
        {

            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenInvalidNewRoleIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();

            var newRole = new IdentityRole("not existing role");

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                                               userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.EditUserRoleAsync(user.Id, newRole.Id));
            }
        }
    }
}

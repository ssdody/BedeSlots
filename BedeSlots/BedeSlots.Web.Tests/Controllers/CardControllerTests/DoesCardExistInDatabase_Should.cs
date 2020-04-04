using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Data.Models;
using BedeSlots.DTO.BankCardDto;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BedeSlots.Web.Tests.Controllers.CardControllerTests
{
    [TestClass]
    public class DoesCardExistInDatabase_Should
    {
        [TestMethod]
        public async Task ReturnsJsonTrue_WhenNumberDoesNotExist()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);
            var userManagerMock = SetupUserManagerMock();

            var user = controller.User;
            var appUser = new User()
            {
                Id = "userId",
            };

            userManagerMock
                .Setup(u => u.GetUserId(user))
                .Returns(appUser.Id);

            var bankCards = new List<CardNumberDto>
            {
                new CardNumberDto{Id = 1, Number = "1"},
                new CardNumberDto{Id = 2, Number = "2"}
            };

            cardServiceMock
                .Setup(c => c.GetUserCardsAllNumbersAsync(It.IsAny<string>()))
                .ReturnsAsync(bankCards);

            // Act
            string cardNumber = "3";
            var result = await controller.DoesCardExistInDatabase(cardNumber);

            // Assert
            Assert.AreEqual(true, result.Value);
        }

        [TestMethod]
        public async Task ReturnsErrorMessage_WhenInvalidAmountPassed()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);
            var userManagerMock = SetupUserManagerMock();

            var user = controller.User;
            var appUser = new User()
            {
                Id = "userId",
            };

            userManagerMock
                .Setup(u => u.GetUserId(user))
                .Returns(appUser.Id);

            var bankCards = new List<CardNumberDto>
            {
                new CardNumberDto{Id = 1, Number = "1"},
                new CardNumberDto{Id = 2, Number = "2"}
            };

            cardServiceMock
                .Setup(c => c.GetUserCardsAllNumbersAsync(It.IsAny<string>()))
                .ReturnsAsync(bankCards);

            // Act
            string cardNumber = "1";
            var result = await controller.DoesCardExistInDatabase(cardNumber);

            string resultMsg = result.Value.ToString();

            // Assert
            StringAssert.Contains(resultMsg, "already added");
        }

        private CardController SetupController(Mock<ICardService> cardServiceMock, Mock<IDateTimeProvider> dateTimeMock)
        {
            var userManagerMock = SetupUserManagerMock();

            var controller = new CardController(userManagerMock.Object, cardServiceMock.Object, dateTimeMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal()
                    }
                },
                TempData = new Mock<ITempDataDictionary>().Object
            };

            return controller;
        }

        private Mock<UserManager<User>> SetupUserManagerMock()
        {
            return new Mock<UserManager<User>>(
                  new Mock<IUserStore<User>>().Object,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null);
        }
    }
}

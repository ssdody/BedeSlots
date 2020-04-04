using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Controllers;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;

namespace BedeSlots.Web.Tests.Controllers.CardControllerTests
{
    [TestClass]
    public class AddCard_Should
    {
        [TestMethod]
        public void ReturnsCorrectViewModel_WhenCalled()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);

            // Act
            var result = controller.AddCard() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.Model, typeof(AddCardViewModel));
        }

        [TestMethod]
        public void InvokeCorrectServiceMethod_WhenCalled()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);

            var viewModel = new AddCardViewModel() { CardNumber = "1111111111111111" };

            // Act
            var result = controller.AddCard(viewModel);

            // Assert
            cardServiceMock.Verify(x => x.AddCardAsync(It.IsAny<string>(), It.IsAny<string>(),
                     It.IsAny<string>(),
                     It.IsAny<DateTime>(),
                     It.IsAny<CardType>(),
                     It.IsAny<string>()
                ), Times.Once);
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

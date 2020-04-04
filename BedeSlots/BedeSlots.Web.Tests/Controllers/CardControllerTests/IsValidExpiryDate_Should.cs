using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;

namespace BedeSlots.Web.Tests.Controllers.CardControllerTests
{
    [TestClass]
    public class IsValidExpiryDate_Should
    {
        [TestMethod]
        public void ReturnsJsonTrue_WhenNumberDoesNotExist()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);

            var expiryDate = new DateTime(2020, 5, 10);
            var dateTimeNow = new DateTime(2018, 12, 12);

            dateTimeProvMock
                .Setup(d => d.Now)
                .Returns(dateTimeNow);

            // Act
            var result = controller.IsValidExpiryDate(expiryDate);

            // Assert
            Assert.AreEqual(true, result.Value);
        }

        [TestMethod]
        public void ReturnsErrorMessage_WhenInvalidAmountPassed()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var dateTimeProvMock = new Mock<IDateTimeProvider>();
            var controller = SetupController(cardServiceMock, dateTimeProvMock);

            var expiryDate = new DateTime(2017, 5, 10);
            var dateTimeNow = new DateTime(2018, 12, 12);

            dateTimeProvMock
                .Setup(d => d.Now)
                .Returns(dateTimeNow);

            // Act
            var result = controller.IsValidExpiryDate(expiryDate);
            string resultMsg = result.Value.ToString();

            // Assert
            StringAssert.Contains(resultMsg, "Invalid expiry date");
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

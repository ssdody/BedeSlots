using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Games.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace BedeSlots.Games.Tests.SlotMachineTests
{
    [TestClass]
    public class GetRandomItem_Should
    {
        [TestMethod]
        public void ReturnsItem_WhenInvoked()
        {
            //Arrange
            var firstItem = new Item
            {
                Name = "item1",
                CumulativeProbability = 50
            };
            
            var items = new Dictionary<int, Item>
            {
                { firstItem.CumulativeProbability, firstItem }
            };

            var randomProviderMock = new Mock<IRandomProvider>();
            randomProviderMock
                .Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(50);
            var sut = new SlotMachine(randomProviderMock.Object);

            // Act
            var result = sut.GetRandomItem(items);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Item));
        }

        [TestMethod]
        public void ReturnsCorrectItem_WhenInvoked()
        {
            //Arrange
            var firstItem = new Item
            {
                Name = "item1",
                CumulativeProbability = 50
            };

            var secondItem = new Item
            {
                Name = "item2",
                CumulativeProbability = 20
            };

            var items = new Dictionary<int, Item>
            {
                { firstItem.CumulativeProbability, firstItem },
                { secondItem.CumulativeProbability, secondItem }
            };

            var randomProviderMock = new Mock<IRandomProvider>();
            randomProviderMock
                .Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(50);
            var sut = new SlotMachine(randomProviderMock.Object);

            // Act
            var result = sut.GetRandomItem(items);

            // Assert
            Assert.AreEqual(firstItem, result);
        }
    }
}

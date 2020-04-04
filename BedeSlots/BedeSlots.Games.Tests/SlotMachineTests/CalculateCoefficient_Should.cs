using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Games.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace BedeSlots.Games.Tests.SlotMachineTests
{
    [TestClass]
    public class CalculateCoefficient_Should
    {
        [TestMethod]
        public void ReturnCorrectCoefficient_WhenInvoked()
        {
            //Arrange
            var firstItem = new Item
            {
                Name = "item1",
                Coefficient = 1,
                CumulativeProbability = 50
            };

            var secondItem = new Item
            {
                Name = "item2",
                Coefficient = 2,
                CumulativeProbability = 20,
                Type = ItemType.Wildcard
            };

            var items = new Dictionary<int, Item>
            {
                { firstItem.CumulativeProbability, firstItem },
                { secondItem.CumulativeProbability, secondItem }
            };

            var matrix = new Item[1, 3];
            matrix[0, 0] = firstItem;
            matrix[0, 1] = firstItem;
            matrix[0, 2] = secondItem;

            var randomProviderMock = new Mock<IRandomProvider>();
            var sut = new SlotMachine(randomProviderMock.Object);

            // Act
            var result = sut.CalculateCoefficient(matrix);

            // Assert
            Assert.AreEqual(2, result);
        }
    }
}

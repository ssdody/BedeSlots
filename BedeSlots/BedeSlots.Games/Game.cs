using BedeSlots.Games.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BedeSlots.Games
{
    public class Game : IGame
    {
        public SpinData Spin(int rows, int cols, decimal money)
        {
            var items = GetItems();
            var matrix = GenerateMatrix(rows, cols, items);
            var coefficient = CalculateCoefficient(matrix);

            if (coefficient != 0)
            {
                money = (decimal)coefficient * money;
            }
            else
            {
                money = 0;
            }

            var spinData = new SpinData()
            {
                Matrix = GetCharMatrix(matrix),
                Money = money
            };

            return spinData;
        }

        private double CalculateCoefficient(Item[,] matrix)
        {
            double finalCoef = 0;

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                Item previousItem = null;
                bool isWinning = true;
                double rowCoef = 0;

                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    //can be optimised - element = GenerateItem()
                    var element = matrix[row, col];

                    if (element.Type == ItemType.Wildcard)
                    {
                        continue;
                    }

                    if (previousItem != null)
                    {
                        if (element != previousItem)
                        {
                            isWinning = false;
                            break;
                        }
                        else
                        {
                            rowCoef += element.Coefficient;
                            previousItem = element;
                        }
                    }
                    else
                    {
                        rowCoef += element.Coefficient;
                        previousItem = element;
                    }
                }

                if (isWinning)
                {
                    finalCoef += rowCoef;
                }
            }

            return finalCoef;
        }

        public Item[,] GenerateMatrix(int rows, int cols, IDictionary<int, Item> items)
        {
            var matrix = new Item[rows, cols];
            items = GetItems();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    matrix[row, col] = GenerateItem(items);
                }
            }

            return matrix;
        }

        public string[,] GetCharMatrix(Item[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            string[,] stringMatrix = new string[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    stringMatrix[row, col] = matrix[row, col].Name;
                }
            }

            return stringMatrix;
        }

        private Item GenerateItem(IDictionary<int, Item> items)
        {
            var random = new Random();
            var randomNumber = random.Next(1, 101); //the maxValue is exclusive

            Item selectedItem = null;
            foreach (var item in items)
            {
                if (randomNumber <= item.Key)
                {
                    selectedItem = item.Value;
                    break;
                }
            }

            return selectedItem;
        }

        private IDictionary<int, Item> GetItems()
        {
            var apple = new Item
            {
                Name = "a",
                Coefficient = 0.4d,
                Probability = 45,
                Type = ItemType.Normal
            };

            var banana = new Item
            {
                Name = "b",
                Coefficient = 0.6d,
                Probability = 35,
                Type = ItemType.Normal
            };

            var pineapple = new Item
            {
                Name = "p",
                Coefficient = 0.8d,
                Probability = 15,
                Type = ItemType.Normal
            };

            var wildcard = new Item
            {
                Name = "w",
                Coefficient = 0d,
                Probability = 5,
                Type = ItemType.Wildcard

            };

            var items = new List<Item>
            {
                apple,
                banana,
                pineapple,
                wildcard
            };

            var sortedItems = items.OrderBy(i => i.Probability).ToList();

            int previousCumulativeProb = 0;
            for (int i = 0; i < sortedItems.Count; i++)
            {
                var currentCumulativeProb = sortedItems[i].Probability + previousCumulativeProb;
                sortedItems[i].CumulativeProbability = currentCumulativeProb;
                previousCumulativeProb = currentCumulativeProb;
            }

            var sortedItemsDict = new SortedDictionary<int, Item>
            {
                { apple.CumulativeProbability, apple },
                { pineapple.CumulativeProbability, pineapple },
                { banana.CumulativeProbability, banana },
                { wildcard.CumulativeProbability, wildcard }
            };

            return sortedItemsDict;
        }
    }
}

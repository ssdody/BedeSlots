
using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Games.Contracts;
using BedeSlots.Games.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BedeSlots.Games.Tests")]
namespace BedeSlots.Games
{
    public class SlotMachine : ISlotMachine
    {
        private readonly List<int> winningRows;
        private readonly IDictionary<int, Item> items;
        private readonly IRandomProvider randomProvider;

        public SlotMachine(IRandomProvider randomProvider)
        {
            this.randomProvider = randomProvider;
            items = GenerateItems.GetItems();
            winningRows = new List<int>();
        }

        public SpinData Spin(int rows, int cols, decimal amount)
        {
            var matrix = GenerateItemMatrix(rows, cols, items);
            var coefficient = CalculateCoefficient(matrix);

            amount =  coefficient != 0 ? Math.Round(((decimal)coefficient * amount),2) : 0;

            var spinData = new SpinData()
            {
                Matrix = GetNamesOfItems(matrix),
                Amount = amount,
                WinningRows = winningRows,
                Coefficient = coefficient
            };

            return spinData;
        }

        public string[,] GenerateMatrixWithItemNames(int rows, int cols)
        {
            string[,] stringMatrix = new string[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    stringMatrix[row, col] = GetRandomItem(items).Name;
                }
            }

            return stringMatrix;
        }

        internal decimal CalculateCoefficient(Item[,] matrix)
        {
            decimal totalCoef = 0;
            winningRows.Clear();

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                Item previousItem = null;
                bool isWinningRow = true;
                decimal rowCoef = 0;

                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    var element = matrix[row, col];
                    if (element.Type == ItemType.Wildcard)
                    {
                        continue;
                    }

                    if (previousItem != null && element != previousItem)
                    {
                        isWinningRow = false;
                        break;
                    }

                    rowCoef += element.Coefficient;
                    previousItem = element;
                }

                if (isWinningRow)
                {
                    totalCoef += rowCoef;
                    winningRows.Add(row);
                }
            }

            return totalCoef;
        }

        private Item[,] GenerateItemMatrix(int rows, int cols, IDictionary<int, Item> items)
        {
            var matrix = new Item[rows, cols];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    matrix[row, col] = GetRandomItem(items);
                }
            }

            return matrix;
        }

        private string[,] GetNamesOfItems(Item[,] matrix)
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
        // key - cumulative probability
        internal Item GetRandomItem(IDictionary<int, Item> items)
        {
            var randomNumber = randomProvider.Next(1, 101); //the maxValue is exclusive

            Item selectedItem = null;
            foreach (var item in items)
            {
                //randomNumber is betwwen 1 and 100
                if (randomNumber <= item.Key)
                {
                    selectedItem = item.Value;
                    break;
                }
            }
            return selectedItem;
        }
    }
}

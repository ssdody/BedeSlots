using BedeSlots.Games.Models;
using System.Collections.Generic;
using System.Linq;

namespace BedeSlots.Games
{
    using static GameConstants;

    public static class GenerateItems
    {
        public static IDictionary<int, Item> GetItems()
        {
            var apple = new Item
            {
                Name = AppleSymbol,
                Coefficient = AppleCoef,
                Probability = AppleProb,
                Type = ItemType.Normal
            };

            var banana = new Item
            {
                Name = BananaSymbol,
                Coefficient = BananaCoef,
                Probability = BananaProb,
                Type = ItemType.Normal
            };

            var pineapple = new Item
            {
                Name = PineappleSymbol,
                Coefficient = PineappleCoef,
                Probability = PineappleProb,
                Type = ItemType.Normal
            };

            var wildcard = new Item
            {
                Name = WildcardSymbol,
                Coefficient = WildcardCoef,
                Probability = WildcardProb,
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

            CalculateCumulativeProbability(sortedItems);

            //Sorted dictionary by cumulative probability
            var sortedItemsDict = new SortedDictionary<int, Item>
            {
                { apple.CumulativeProbability, apple },
                { pineapple.CumulativeProbability, pineapple },
                { banana.CumulativeProbability, banana },
                { wildcard.CumulativeProbability, wildcard }
            };

            return sortedItemsDict;
        }

        private static void CalculateCumulativeProbability(List<Item> sortedItems)
        {
            int previousCumulativeProb = 0;
            for (int i = 0; i < sortedItems.Count; i++)
            {
                var currentCumulativeProb = sortedItems[i].Probability + previousCumulativeProb;
                sortedItems[i].CumulativeProbability = currentCumulativeProb;
                previousCumulativeProb = currentCumulativeProb;
            }
        }
    }
}

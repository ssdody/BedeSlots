using BedeSlots.Common.Providers.Contracts;
using System;

namespace BedeSlots.Common.Providers
{
    public class RandomProvider : IRandomProvider
    {
        public int Next(int minValue, int maxValue)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(minValue, maxValue);
        }
    }
}


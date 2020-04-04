using System.Collections.Generic;

namespace BedeSlots.Games.Models
{
    public class SpinData
    {
        public string[,] Matrix { get; set; }

        public decimal Amount { get; set; }

        public List<int> WinningRows { get; set; }

        public decimal Coefficient { get; set; }
    }
}

namespace BedeSlots.Games
{
    public class Item
    {
        public string Name { get; set; }

        public double Coefficient { get; set; }

        public int Probability { get; set; }

        public int CumulativeProbability { get; set; }

        public ItemType Type { get; set; }
    }
}

namespace BedeSlots.Web.Models
{
    public class GameSlotViewModel
    {
        public int Rows { get; set; }

        public int Cols { get; set; }

        public string[,] Matrix { get; set; }

        public decimal Money { get; set; }
    }
}

using BedeSlots.Data.Models;

namespace BedeSlots.Web.Models
{
    public class UserBalanceViewModel
    {
        public UserBalanceViewModel()
        {
        }

        public decimal Balance { get; set; }

        public Currency Currency { get; set; }
    }
}

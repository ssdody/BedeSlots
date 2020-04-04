using BedeSlots.Data.Models;
using System;

namespace BedeSlots.Web.Areas.Admin.Models
{
    public class TransactionHistoryViewModel
    {
        public TransactionHistoryViewModel()
        {
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public TransactionType Type { get; set; }

        public GameType? GameType { get; set; }
        
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string UserEmail { get; set; }
    }
}

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

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string UserEmail { get; set; }
    }
}

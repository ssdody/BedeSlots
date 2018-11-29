using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Areas.Admin.Models
{
    public class TransactionDetailsViewModel
    {
        public TransactionDetailsViewModel()
        {

        }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserEmail { get; set; }

        public DateTime Birthdate { get; set; }

        public ICollection<string> Cards { get; set; }
    }
}

using BedeSlots.Common;
using BedeSlots.Data.Models;
using BedeSlots.DTO.TransactionDto;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Areas.Admin.Models;
using BedeSlots.Web.Infrastructure.Providers.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Areas.Admin.Controllers
{
    [Area(WebConstants.AdminArea)]
    [Authorize(Roles = WebConstants.AdminRole + "," + WebConstants.MasterAdminRole)]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService transactionService;
        private readonly ICardService cardService;
        private readonly IPaginationProvider<TransactionManageDto> paginationProvider;

        public TransactionsController(ITransactionService transactionService, ICardService cardService, IPaginationProvider<TransactionManageDto> paginationProvider)
        {
            this.transactionService = transactionService;
            this.cardService = cardService;
            this.paginationProvider = paginationProvider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoadData()
        {
            try
            {
                string draw, sortColumn, sortColumnDirection, searchValue;
                int pageSize, skip, recordsTotal;

                this.paginationProvider.GetParameters(out draw, out sortColumn, out sortColumnDirection, out searchValue, out pageSize, out skip, out recordsTotal, HttpContext, Request);

                var transactions = this.transactionService.GetAllTransactions();

                //Search
                if (!String.IsNullOrEmpty(searchValue.ToLower()))
                {
                    transactions = transactions
                        .Where(t => t.User.ToLower().Contains(searchValue)
                        || t.Description.ToLower().Contains(searchValue));
                }

                //Sorting
                transactions = this.paginationProvider.SortData(sortColumn, sortColumnDirection, transactions);

                //Total number of rows count 
                recordsTotal = transactions.Count();

                //Paging 
                var dataFiltered = transactions
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var data = dataFiltered
                    .Select(t => new
                    {
                        Date = t.Date.ToString("G", CultureInfo.InvariantCulture),
                        Type = t.Type.ToString(),
                        Amount = CommonConstants.BaseCurrencySymbol + t.Amount.ToString(),
                        Description = GetDescriptionByTransactionType(t.Type) + t.Description,
                        User = t.User
                    })
                    .ToList();

                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception)
            {
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            }
        }

        private string GetDescriptionByTransactionType(TransactionType type)
        {
            if (type == TransactionType.Deposit || type == TransactionType.Withdraw)
            {
                return $"{type.ToString()} with card **** **** **** ";
            }
            else
            {
                return $"{type.ToString()} on game ";
            }
        }
    }
}

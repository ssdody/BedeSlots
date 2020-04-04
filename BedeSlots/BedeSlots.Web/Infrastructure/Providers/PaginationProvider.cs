using BedeSlots.Web.Infrastructure.Providers.Contracts;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BedeSlots.Web.Infrastructure.Providers
{
    public class PaginationProvider<T> : IPaginationProvider<T>
    {
        public void GetParameters(out string draw, out string sortColumn, out string sortColumnDirection, out string searchValue, out int pageSize, out int skip, out int recordsTotal, HttpContext httpContext, HttpRequest httpRequest)
        {
            draw = httpContext.Request.Form["draw"].FirstOrDefault();
            // Skiping number of Rows count
            var start = httpRequest.Form["start"].FirstOrDefault();
            // Paging Length 10,20
            var length = httpRequest.Form["length"].FirstOrDefault();
            // Sort Column Name
            sortColumn = httpRequest.Form["columns[" + httpRequest.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction ( asc ,desc)
            sortColumnDirection = httpRequest.Form["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)
            searchValue = httpRequest.Form["search[value]"].FirstOrDefault().ToLower();

            //Paging Size (10,20,50,100)
            pageSize = length != null ? int.Parse(length) : 0;
            skip = start != null ? int.Parse(start) : 0;
            recordsTotal = 0;
        }

        public IQueryable<T> SortData(string sortColumn, string sortColumnDirection, IQueryable<T> collection)
        {
            //Sorting
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                if (sortColumnDirection == "asc")
                {
                    collection = collection
                        .OrderBy(u => u.GetType().GetProperty(sortColumn).GetValue(u));
                }
                else
                {
                    collection = collection
                        .OrderByDescending(u => u.GetType().GetProperty(sortColumn).GetValue(u));
                }
            }

            return collection;
        }
    }
}

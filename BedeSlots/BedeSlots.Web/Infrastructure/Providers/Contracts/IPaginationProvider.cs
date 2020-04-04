using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BedeSlots.Web.Infrastructure.Providers.Contracts
{
    public interface IPaginationProvider<T>
    {
        void GetParameters(out string draw, out string sortColumn, out string sortColumnDirection, out string searchValue, out int pageSize, out int skip, out int recordsTotal, HttpContext httpContext, HttpRequest httpRequest);

        IQueryable<T> SortData(string sortColumn, string sortColumnDirection, IQueryable<T> collection);
    }
}
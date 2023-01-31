using Microsoft.EntityFrameworkCore;

namespace BusReservationSystemApi.Utils
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        ///<summary>
        ///     Creates a paginated list using the data from specified source.
        ///</summary>
        ///<param name="source"> The source of data i.e the data you're returning as list</param>
        ///<param name="pageIndex"> The page number</param>
        ///<param name="pageSize"> number of items to be displayed in a page. Defaults to 10.</param>
        ///<returns>List of items with the size of pageSize.</returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize = 10)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);

        }
    }
}

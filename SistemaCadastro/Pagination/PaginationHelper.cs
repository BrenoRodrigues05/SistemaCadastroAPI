using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Models;

namespace SistemaCadastro.Pagination
{
    public class PaginationHelper
    {
        public static async Task<PagedResult<T>> GetPagedResultAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}

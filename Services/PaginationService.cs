using Exist.ViewModels.Pagination;
using System.Linq;

namespace Exist.Services
{
    public static class PaginationService<T>
    {
        private static int _pageSize = 3;

        public static IndexViewModel<T> Do(IQueryable<T> sortedResult, int page, string url)
        {
            var phonesPerPages = sortedResult.Skip((page - 1) * _pageSize).Take(_pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = _pageSize, TotalItems = sortedResult.Count() };
            IndexViewModel<T> result = new IndexViewModel<T> { PageInfo = pageInfo, Models = phonesPerPages };

            if (result.PageInfo.TotalPages > page)
            {
                result.PageInfo.NextPage = url + $"page={page + 1}";
            }

            if (page > 1)
            {
                result.PageInfo.PreviousPage = url + $"page={page - 1}";
            }

            return result;
        }
    }
}

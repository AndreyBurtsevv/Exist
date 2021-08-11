using System.Collections.Generic;

namespace Exist.ViewModels.Pagination
{
    public class IndexViewModel<T>
    {
        public IEnumerable<T> Models { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}

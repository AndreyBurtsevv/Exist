using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.ViewModels.Pagination
{
    public class IndexViewModel<T>
    {
        public IEnumerable<T> Models { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}

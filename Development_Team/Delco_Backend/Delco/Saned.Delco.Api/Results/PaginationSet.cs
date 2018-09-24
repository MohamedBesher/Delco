using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saned.Delco.Api.Results
{
    public class PaginationSet<T>
    {
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }

        public int Count => Items?.Count() ?? 0;
        public int Page { get; set; }
    }
}
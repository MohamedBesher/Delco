using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saned.Delco.Api.Models
{
    public class PagedParam
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Keyword { get; set; }
    }
}
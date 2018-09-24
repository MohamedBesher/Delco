using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saned.Delco.Api.Models
{
    public class PagingViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
    }

  
}
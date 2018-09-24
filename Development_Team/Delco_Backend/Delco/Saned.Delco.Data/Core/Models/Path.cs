using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Data.Core.Models
{
    public class Path
    {
        public long Id { get; set; }   
        public decimal Price { get; set; }
       
        public PathTypesEnum Type { get; set; }
        public long FromCityId { get; set; }
        public long? ToCityId { get; set; }
        public virtual City FromCity { get; set; }
        public virtual City ToCity { get; set; }
    }
}

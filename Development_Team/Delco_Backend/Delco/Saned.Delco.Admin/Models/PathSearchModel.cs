using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Admin.Models
{
    public class PathSearchModel : Pager
    {
     
        public int Id { get; set; }
        public long FromCityId { get; set; } = 0;
        public long ToCityId { get; set; } = 0;
    }
}
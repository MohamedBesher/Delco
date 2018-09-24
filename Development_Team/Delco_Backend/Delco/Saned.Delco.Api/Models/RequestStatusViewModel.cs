using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Api.Models
{
    public class RequestStatusViewModel : PagingViewModel
    {
        public int? Status { get; set; }
        public int? Type { get; set; }
    }
}
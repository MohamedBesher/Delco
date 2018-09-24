using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Admin.Models
{
    public class RequestSearchModel:Pager
    {

        public long CityId { get; set; }
        //public string UserId { get; set; }
        public string AgentId { get; set; }
        public RequestTypeEnum? Type { get; set; }
        public RequestStatusEnum? Status { get; set; }


        //public int Page { get; set; } = 1;
        //public int PageSize { get; set; } = 10;
        //public string Keyword { get; set; } = string.Empty;
        //public long Id { get; set; }
        //public string UserId { get; set; }


        //Id = item.Id,
        //                        Page = ViewBag.Page,
        //                        Keyword = ViewBag.Keyword,
        //                        PageSize = ViewBag.PageSize,
        //                        UserId = ViewBag.UserId,
        //                        AgentId = ViewBag.AgentId,
        //                        Status = ViewBag.Status,
        //                        CityId = ViewBag.CityId,




    }
}
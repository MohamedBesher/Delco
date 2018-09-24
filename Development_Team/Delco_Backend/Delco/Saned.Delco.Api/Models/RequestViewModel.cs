using System;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Api.Models
{
    public class RequestViewModel
    {
        public RequestViewModel()
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            CreatedDate = now;
        }
        public long? Id { get; set; }
        public string Address { get; set; }
        public string FromLongtitude { get; set; }
        public string FromLatitude { get; set; }
        public string FromLocation { get; set; }

        public string ToLongtitude { get; set; }
        public string ToLatitude { get; set; }
        public string ToLocation { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public long? CityId { get; set; }

        public string UserId { get; set; }
        public string AgentId { get; set; }


        public long? PassengerNumberId { get; set; }
        public DateTime CreatedDate { get; private set; }
        public int Status { get; set; }
        public int Type { get; set; }

    }

    public class EditRequestViewModel
    {
        public long Id { get; set; }
    }
}
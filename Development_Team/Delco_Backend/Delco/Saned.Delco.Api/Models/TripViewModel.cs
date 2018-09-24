using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Api.Models
{
    public class TripViewModel
    {
        public long Id { get; set; }
        public string FromLongtitude { get; set; }
        public string FromLatitude { get; set; }
        public string FromLocation { get; set; }

        public string ToLongtitude { get; set; }
        public string ToLatitude { get; set; }
        public string ToLocation { get; set; }

        public decimal Price { get; set; }
        public int PassengerNumber { get; set; }
        public long CityId { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public string Status { get; set; }


    }
}
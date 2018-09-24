using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saned.Delco.Api.Models
{
    public class CalculateTripPriceViewModel
    {
        public string FromLatitude { get; set; }
        public string FromLongitude { get; set; }
        public string ToLatitude { get; set; }
        public string ToLongitude { get; set; }
    }


    public class CalculateOrderViewModel
    {
        public int CityId { get; set; }
        public string ToLatitude { get; set; }
        public string ToLongitude { get; set; }
    }
}
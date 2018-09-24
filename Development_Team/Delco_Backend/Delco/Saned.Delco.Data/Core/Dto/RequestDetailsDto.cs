using System;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Core.Dto
{
    public class RequestDetailsDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public string UserPhoneNumber { get; set; }
        public string AgentName { get; set; }
        public string AgentPhoneNumber { get; set; }
        public int? Degree { get; set; }
        public int? TotalDegree { get; set; }

        public string Address { get; set; }
        public string FromLongtitude { get; set; }
        public string FromLatitude { get; set; }
        public string FromLocation { get; set; }

        public string ToLongtitude { get; set; }
        public string ToLatitude { get; set; }
        public string ToLocation { get; set; }
        public int  Status { get; set; }
        public decimal Price { get; set; }
        public string RequestCity { get; set; }
        public string Description { get; set; }
        public string PassengerNumber { get; set; }

        public string RefuseReason { get; set; }
    }
}
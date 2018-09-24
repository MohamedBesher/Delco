using System;

namespace Saned.Delco.Api.Models
{
    public class RatingViewModel
    {
        public long Id { get; set; }
        public int TotalDegree { get; set; }
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Degree { get; set; }
    }
}
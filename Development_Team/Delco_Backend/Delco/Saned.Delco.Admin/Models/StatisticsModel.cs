namespace Saned.Delco.Admin.Models
{
    public class StatisticsModel
    {

        public int UserCount { get; set; }
        public int AgentCount { get; set; }
        public int TripCount { get; set; }

        public int RequestCount { get; set; }
        public int RequestNew { get; set; }
        public int RequestInProgress { get; set; }
        public int RequestCanceled { get; set; }
        public int RequestDelivered { get; set; }
        public int TripNew { get; set; }
        public int TripInProgress { get; set; }
        public int TripCanceled { get; set; }
        public int TripDelivered { get; set; }
        public int InternalPath { get; set; }
        public int ExternalPath { get; set; }
        public int AgentRequestsCount { get; set; }
        public int AgentRequestsUnSeenCount { get; set; }
        public int CityCount { get; set; }
    }
}
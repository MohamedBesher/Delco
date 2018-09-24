namespace Saned.Delco.Api.Models
{
    public class RefuseRequestModel
    {
        public long Id { get; set; }

        public string UserId { get; set; }
        public string AgentId { get; set; }
        
        public long RequestId { get; set; }
        public long? RefuseReasonId { get; set; }
        public string Cause { get; set; }
    }
}
namespace Saned.Delco.Data.Core.Models
{
    public class RefuseRequest
    {
        public long Id { get; set; }
    
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public long? RefuseReasonId { get; set; }
        public long RequestId { get; set; }
        public string Cause { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Agent { get; set; }
        public virtual RefuseReason RefuseReason { get; set; }
        public virtual Request Request { get; set; }
        
    }
}
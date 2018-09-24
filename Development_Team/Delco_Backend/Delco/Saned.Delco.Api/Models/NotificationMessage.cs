namespace Saned.Delco.Api.Models
{
    public class NotificationMessage
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string EnglishMessage { get; set; }
        public long? TripId { get; set; }
   
        public long? RequestId { get; set; }
  
   
    }
}
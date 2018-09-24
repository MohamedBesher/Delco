using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Delco.Data.Persistence.Infrastructure;

namespace Saned.Delco.Data.Core.Models
{
    public class NotificationMessage 
    {
        public NotificationMessage()
        {
            CreationDate = DateTime.Now;
            NotificationDevices = new Collection<NotificationDevice>();
        }

        public long Id { get; set; }
        public string Message { get; set; }
        public string EnglishMessage { get; set; }
        public DateTime CreationDate { get; private set; }

       // public long? TripId { get; set; }
        //public virtual Trip Trip { get; set; }
        public long? RequestId { get; set; }
        public virtual Request Request { get; set; }
        public ICollection<NotificationDevice> NotificationDevices { get; set; }

        public void Modify(string message, string englishMessage)
        {
            Message = message;
            EnglishMessage = englishMessage;
        }


    }
}

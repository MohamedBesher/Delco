using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Persistence.Infrastructure;

namespace Saned.Delco.Data.Core.Models
{
    public class NotificationDevice 
    {
        public long Id { get; set; }
        public long NotificationMessageId { get; set; }
        public string ApplicationUserId { get; set; }
        public bool IsRead { get; set; } = false;
        public NotificationType Type { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual NotificationMessage NotificationMessage { get; set; }

    }
}

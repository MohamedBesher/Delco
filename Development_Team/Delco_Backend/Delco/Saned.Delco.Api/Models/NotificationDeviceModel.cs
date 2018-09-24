using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Api.Models
{
    public class NotificationDeviceModel
    {
        public long Id { get; set; }
        public long NotificationMessageId { get; set; }
        public string ApplicationUserId { get; set; }
        public NotificationType Type { get; set; }
    }
}
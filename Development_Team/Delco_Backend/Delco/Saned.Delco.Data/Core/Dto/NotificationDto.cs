using System;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Extentions;

namespace Saned.Delco.Data.Core.Dto
{
    public class NotificationDto
    {
        //public NotificationDto()
        //{
        //    Color = (this.Type).GetColorValue();
        //    Icon = (this.Type).GetIconValue();
        //}

        public string Message { get; set; }
        public long? Request { get; set; }

        public string Icon => (this.Type).GetIconValue();

        public string Color => (this.Type).GetColorValue();
        public bool IsRead { get; set; }
        public DateTime CreationDate { get; set; }
        public NotificationType Type { get; set; }
    }
}

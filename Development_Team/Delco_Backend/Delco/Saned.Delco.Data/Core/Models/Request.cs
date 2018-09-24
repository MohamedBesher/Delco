using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Persistence.Infrastructure;

namespace Saned.Delco.Data.Core.Models
{
    public class Request
    {
        public Request()
        {
            //DateTime timeUtc = System.DateTime.UtcNow;
            //TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);


            var zone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            CreatedDate = now;

            NotificationMessages = new Collection<NotificationMessage>();

            //  CreatedDate = DateTime.Now;
        }

        public long Id { get; set; }
        public string Address { get; set; }
        public string FromLongtitude { get; set; }
        public string FromLatitude { get; set; }
        public string FromLocation { get; set; }

        public string ToLongtitude { get; set; }
        public string ToLatitude { get; set; }
        public string ToLocation { get; set; }

        public decimal Price { get; set; }
       
        public long? CityId { get; set; }
       
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public long? PassengerNumberId { get; set; }
        public string Description { get; set; }
        public RequestStatusEnum Status { get; set; }
        public bool IsHidden { get; set; } = false;
        public RequestTypeEnum Type { get; set; }

        public DateTime CreatedDate { get;private set; }


        public virtual PassengerNumber PassengerNumber { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Agent { get; set; }
        public virtual City  City{ get; set; }
        public ICollection<NotificationMessage> NotificationMessages { get; set; }


        public void UpdateStatus(RequestStatusEnum status, RequestTypeEnum type, string description)
        {
            this.Status = status;
            this.Type = type;
            this.Description = description;
        }


        public void CancelVsSetUserToNull()
        {
            this.Status = RequestStatusEnum.Canceled;
            this.UserId = null;
        }
    }






}

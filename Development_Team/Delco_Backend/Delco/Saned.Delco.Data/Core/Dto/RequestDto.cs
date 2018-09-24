using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Core.Dto
{
    public class RequestDto
    {
        public Request Request { get; set; }
        public City City { get; set; }
        public string CityName { get; set; }
    }

    public class RequestListDto
    {
        public long Id { get; set; }

        public string Address { get; set; }
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public DateTime CreatedDate { get;  set; }

    
        public string AgentUserName { get; set; }
        public string AgentFullName { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string CityName { get; set; }

        public RequestStatusEnum Status { get; set; }
        public RequestTypeEnum Type { get; set; }
        public long? CityId { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsHidden { get; set; }

        public PassengerNumber PassengerNumber { get; set; }
        public string UserPhoneNumber { get; set; }
    }
}

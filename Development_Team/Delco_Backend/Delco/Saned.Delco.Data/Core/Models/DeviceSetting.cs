using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Delco.Data.Persistence.Infrastructure;

namespace Saned.Delco.Data.Core.Models
{
    public class DeviceSetting
    {
        public DeviceSetting()
        {
          
        }

        public long Id { get; set; }
        public string DeviceId { get; set; }
        public string ApplicationUserId { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }

    }


}

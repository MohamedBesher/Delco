using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Persistence.Infrastructure;
using System.Collections.Generic;

namespace Saned.Delco.Data.Core.Models
{
    public class PassengerNumber
    {
        public long Id { get; set; }
        public long Value { get; set; }
        public string Name { get; set; }

        public void UpdateStatus(long value , string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public virtual ICollection<Request> Requestes { get; set; }


    }
}
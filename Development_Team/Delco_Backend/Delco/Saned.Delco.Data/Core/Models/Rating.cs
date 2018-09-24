using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Delco.Data.Persistence.Infrastructure;

namespace Saned.Delco.Data.Core.Models
{
    
    public class Rating
    {
        public Rating()
        {
            Date = DateTime.Now;
        }
        public long Id { get; set; }
        public int TotalDegree { get; set; }
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public DateTime Date { get; set; }
        public int Degree { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Agent { get; set; }
    }
}

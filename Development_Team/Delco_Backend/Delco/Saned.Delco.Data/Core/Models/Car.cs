using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Delco.Data.Core.Models
{
    public class Car
    {
       
        [Key, ForeignKey("User")]
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string PlateNumber { get; set; }
        public long PassengerNumberId { get; set; }

        public virtual  PassengerNumber PassengerNumber { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
}

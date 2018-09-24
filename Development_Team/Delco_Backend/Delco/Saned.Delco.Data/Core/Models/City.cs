using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Delco.Data.Core.Models
{
    public class City
    {
        public long Id { get; set; }
        [Display(Name = "الاسم")]
        public string Name { get; set; }
        [Display(Name = "خط الطول")]
        public string Latitude { get; set; }
        [Display(Name = "دائرة العرض")]
        public string Longitude { get; set; }
        public int NumberOfKilometers { get; set; }
    }
}

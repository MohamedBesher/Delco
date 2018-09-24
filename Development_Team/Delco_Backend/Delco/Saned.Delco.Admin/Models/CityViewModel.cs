using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Saned.Delco.Admin.Models
{
    public class CityViewModel
    {
        [ScaffoldColumn(false)]
        public long Id { get; set; }

        [Display(Name = "الاسم")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [StringLength(250, ErrorMessage = "لا يجب ان يزيد الاسم عن 250 حرف")]
        public string Name { get; set; }

        [Display(Name = "خط العرض")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        //[StringLength(50, ErrorMessage = "لا يجب ان يزيد خط العرض عن 50 حرف")]

        [Range(0, double.MaxValue, ErrorMessage = "من فضلك ادخل خط عرض صحيح")]
        public string Latitude { get; set; }

        [Display(Name = "خط الطول")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Range(0, double.MaxValue, ErrorMessage = "من فضلك ادخل خط طول صحيح ")]

        public string Longitude { get; set; }

        [Display(Name = "عدد الكيلومترات")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Range(1, int.MaxValue, ErrorMessage = "من فضلك ادخل رقم صحيح (أكبر من 0) ")]
        public int NumberOfKilometers { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Saned.Delco.Admin.CustomValidators;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Admin.Models
{
    public class ExternalPathViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "من مدينة")]
        [CampareWithToCity(ErrorMessage="لا يجب ان تتطابق {0} مع مدينة الوصول")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        public long FromCityId { get; set; }

        [Display(Name = "الى مدينة")]
        [ExistingPath(ErrorMessage="هذا المسار موجود بالفعل")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        public long? ToCityId { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "السعر")]
        [Range(1, int.MaxValue, ErrorMessage = "   من فضلك ادخل رقم صحيح أكبر من 0 ولا يزيد عن  2147483647")]


        [Required(ErrorMessage = "يجب ادخال {0}")]

        public decimal Price { get; set; }

        public  City FromCity { get; set; }
        public  City ToCity { get; set; }
        public PathTypesEnum Type { get; set; }
    }
}
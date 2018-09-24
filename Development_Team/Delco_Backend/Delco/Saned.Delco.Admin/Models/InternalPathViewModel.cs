using System.ComponentModel.DataAnnotations;
using Saned.Delco.Admin.CustomValidators;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Admin.Models
{
    public class InternalPathViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "مدينة")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [ExistingInternalPath(ErrorMessage = "هذا المسار موجود بالفعل")]

        public long FromCityId { get; set; }
       

        [DataType(DataType.Currency)]
        [Display(Name = "السعر")]
        [Range(1, int.MaxValue, ErrorMessage = "   من فضلك ادخل رقم صحيح أكبر من 0 ولا يزيد عن  2147483647")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        public decimal Price { get; set; }

        public City FromCity { get; set; }
        public City ToCity { get; set; }
        public PathTypesEnum Type { get; set; }
    }
}
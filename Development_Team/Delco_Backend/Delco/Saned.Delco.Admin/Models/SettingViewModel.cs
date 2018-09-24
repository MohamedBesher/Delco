using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Admin.Models
{
    public class SettingViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "رسالة المسارات غير المدعومة")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        public string UnSupportedPathMessage { get; set; }
        [Display(Name = "اشعار حظر المندوب")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        public string SuspendAgentMessage { get; set; }
        [Display(Name = "الحد الادنى ")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        
        [Range(0, double.MaxValue, ErrorMessage = "ادخل قيمة صحيحة")]
        public decimal MinimumPrice { get; set; }

        [Display(Name = " بداية الحساب ")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Range(0, double.MaxValue, ErrorMessage = "ادخل قيمة صحيحة")]
        public decimal StartPrice { get; set; }
        [Display(Name = " قيمة الكيلومتر ")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Range(0, double.MaxValue, ErrorMessage = "ادخل قيمة صحيحة")]
        public decimal KiloMeterPrice { get; set; }
        [Display(Name = " قيمة الدقيقة ")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Range(0,(double) decimal.MaxValue, ErrorMessage = "ادخل قيمة صحيحة")]
        public decimal MinutePrice { get; set; }

        [Display(Name = " نسبة الادارة ")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Range(0, double.MaxValue, ErrorMessage = "ادخل قيمة صحيحة")]
        public decimal ManagementPercentage { get; set; }
        [Display(Name = " البريد الإلكتروني لتلقي بلاغات الإساءة")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [DataType(DataType.EmailAddress, ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string AbuseEmail { get; set; }
        [Display(Name = " البريد الإلكتروني لتلقي رسائنل تواصل معنا   ")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [DataType(DataType.EmailAddress, ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string ContactUsEmail { get; set; }
        [Display(Name = " شروط استخدام التطبيق   ")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        //[StringLength(2000, ErrorMessage = "لا يتجاوز 500 حرف")]


        public string TermsOfConditions { get; set; }
    }
}
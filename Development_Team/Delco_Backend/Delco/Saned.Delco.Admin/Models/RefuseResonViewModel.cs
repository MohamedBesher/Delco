using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Admin.Models
{
    public class RefuseResonViewModel
    {
        [Required(ErrorMessage = "يجب ادخال {0}")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "سبب الرفض لا يقل 3 حروف ولا يزيد عن 250  حرف")]
        [Display(Name = "سبب الرفض")]
        public string Value { get; set; }
        public long Id { get; set; }
    }
}
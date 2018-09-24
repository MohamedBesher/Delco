using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Admin.Models
{
    public class RefuseRequestViewModel
    {
        [Required(ErrorMessage = ("سبب الرفض مطلوب"))]
        public long RefuseReasonId { get; set; }

        [Required(ErrorMessage = ("المشور مطلوب"))]

        public long Id { get; set; }

        [StringLength(250,  ErrorMessage = "سبب الرفض لا يزيد عن 250  حرف")]

        public string Cause { get; set; }


    }
}
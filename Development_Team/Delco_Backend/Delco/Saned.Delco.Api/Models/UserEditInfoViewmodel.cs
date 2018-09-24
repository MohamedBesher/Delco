using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Api.Models
{
    public class UserEditInfoViewmodel
    {
        [Required(ErrorMessage = "الاسم بالكامل مطلوب")]
        [Display(Name = "الاسم بالكامل")]
        public string FullName { get; set; }


        [Display(Name = "البريد الالكترونى")]
        [Required(ErrorMessage = "البريد الالكترونى مطلوب")]
        public string Email { get; set; }
    }
}
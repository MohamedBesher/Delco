using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }


        [Display(Name = "البريد الالكترونى")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Display(Name = "رقم الجوال")]

        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Display(Name = "الاسم بالكامل")]
        public string FullName { get; set; }



        [Required(ErrorMessage = "يجب ادخال {0}")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
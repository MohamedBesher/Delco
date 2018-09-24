using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Api.Validators
{
    public class UserViewModel
    {
        public UserViewModel()
        {

        }
        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

       
        [Display(Name = "البريد الالكترونى")]
        [Required(ErrorMessage = "مطلوب")]
        public string Email { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "رقم الجوال")]

        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "الاسم بالكامل")]
        public string FullName { get; set; }
     
        

        [Required(ErrorMessage = "مطلوب")]
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
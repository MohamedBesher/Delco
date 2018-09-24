using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Admin.Models
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }


        [Display(Name = "البريد الالكترونى")]
        [Required(ErrorMessage = "يجب ادخال {0}")]
      //  [System.Web.Mvc.Remote("CheckExistingEmail", "User", HttpMethod = "POST", ErrorMessage = "Email already exists")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Display(Name = "رقم الجوال")]

        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "يجب ادخال {0}")]
        [Display(Name = "الاسم بالكامل")]
        public string FullName { get; set; }



 
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
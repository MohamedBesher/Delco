using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Api.Models
{
    public class ConfirmViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Code { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }





        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
    }
    public class ResetPasswordViewModel
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }




        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }



        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }


    public class AdminChangePasswordViewModel
    {
        public string UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
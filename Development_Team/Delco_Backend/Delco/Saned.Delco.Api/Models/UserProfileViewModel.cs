using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Api.Models
{
    public class UserProfileViewModel

    {
     
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string FullName { get; set; }

      
      
        [Required(ErrorMessage = "البريد الالكترونى مطلوب")]
        public string Email { get; set; }

    }


    public class AgentProfileViewModel: UserProfileViewModel

    {
        [Required(ErrorMessage = "نوع الطلبات المفضلة مطلوب")]

        public int RequestType { get; set; }

        [Required(ErrorMessage = "شركة السيارة مطلوب")]

        public string CompanyName { get; set; }
        [Required(ErrorMessage = "نوع السيارة مطلوب")]

        public string Type { get; set; }
        [Required(ErrorMessage = "موديل السيارة مطلوب")]

        public string Model { get; set; }
        [Required(ErrorMessage = "لون السيارة مطلوب")]

        public string Color { get; set; }
        [Required(ErrorMessage = "رقم لوحة السيارة مطلوب")]

        public string PlateNumber { get; set; }

    }

    public class UserPhotoViewModel

    {
        [Required(ErrorMessage = "صورة المستخدم مطلوب")]

        public string Picture { get; set; }

    }

    public class UserInfoViewModel
    {
        [Required]

        public string UserId { get; set; }

    }
    public class UserNotifyViewModel
    {     
        public bool IsNotify { get; set; }

    }
}
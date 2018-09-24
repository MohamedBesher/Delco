using System.ComponentModel.DataAnnotations;
using Saned.Delco.Api.Validators;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Api.Models
{
    public class UserEditViewmodel
    {

        public string Id { get; set; }
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [UserNameValidator]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "الاسم بالكامل مطلوب")]
        [Display(Name = "الاسم بالكامل")]
        public string FullName { get; set; }


        [Display(Name = "البريد الالكترونى")]
        [Required(ErrorMessage = "البريد الالكترونى مطلوب")]
        public string Email { get; set; }


        public string ConfirmedToken { get; set; }
        public string ResetPasswordlToken { get; set; }
        public bool? IsDeleted { get; set; }

        public string PhotoUrl { get; set; }
        public string Address { get; set; }
        public bool? IsNotified { get; set; }
        public bool? IsSuspend { get; set; }
        public string PhoneNumber { get; set; }
        public int? Type { get; set; }
      
        public long? CityId { get; set; }

        public string CompanyName { get; set; }
        public string CarType { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string PlateNumber { get; set; }
        public long? PassengerNumberId { get; set; }
        public string Role { get; set; }
        public AgentStatisticsModel AgentStatisticsModel  { get; set; }
}




}
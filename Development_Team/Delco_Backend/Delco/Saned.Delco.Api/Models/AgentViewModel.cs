using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Api.Validators
{
    public class AgentViewModel:UserViewModel
    {
        public AgentViewModel()
        {

        }

      
       
        [Required(ErrorMessage = " المدينة مطلوب")]

        public long CityId { get; set; }
      
       
      



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

        [Required(ErrorMessage = "عدد الركاب مطلوب")]

        public int PassengerCount { get; set; }

    }
}
using System.ComponentModel.DataAnnotations;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Admin.Models
{
   

    public class AgentViewModel : UserEditViewModel
    {
   



        [Required(ErrorMessage = " المدينة مطلوب")]
        [Display(Name = "المدينة")]

        public long CityId { get; set; }






        [Required(ErrorMessage = "نوع الطلبات المفضلة مطلوب")]
        [Display(Name = "نوع الطلبات ")]
        public int RequestType { get; set; }
        
        public string TypeRequest { get; set; }
        public string Cityname { get; set; }
        public string Rate { get; set; }
        public string PassengerNumber { get; set; }

        [Required(ErrorMessage = "شركة السيارة مطلوب")]
        [Display(Name = "شركة السيارة")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "نوع السيارة مطلوب")]
        [Display(Name = "نوع السيارة")]
        public string Type { get; set; }
        [Required(ErrorMessage = "موديل السيارة مطلوب")]
        [Display(Name = "موديل السيارة")]
        public string Model { get; set; }
        [Required(ErrorMessage = "لون السيارة مطلوب")]
        [Display(Name = "لون السيارة")]
        public string Color { get; set; }
        [Required(ErrorMessage = "رقم لوحة السيارة مطلوب")]
        [Display(Name = "رقم لوحة السيارة")]
        public string PlateNumber { get; set; }

        [Required(ErrorMessage = "عدد الركاب مطلوب")]
        [Display(Name = "عدد الركاب")]
        public long? PassengerNumberId { get; set; }




     
        [Display(Name = "عدد الطلبات المنفذة")]
        public string ExecutedRequests { get; set; }



        [Display(Name = "إجمالي قيمة التوصيل")]
        public string Total { get; set; }


        [Display(Name = "نسبة الإدارة ")]
        public string AppPercentage { get; set; }



        [Display(Name = "صافي الربح ")]
        public string TotalAfterApp { get; set; }

    }
}
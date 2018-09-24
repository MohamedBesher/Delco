using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Data.Core.Enum
{
    public enum RequestTypesEnum
    {
        [Display(Name = "مشوار")]
        [StringValue(@"مشوار")]
        Trip = 1,

        [Display(Name = "طلب")]
        [StringValue(@"طلب")]
        Request =2,
       
        [Display(Name = "كلاهما")]
        [StringValue(@"كلاهما")]
        Both =3
    }
    public enum RequestTypeEnum
    {
        Trip = 1,
        Request = 2
    }
}
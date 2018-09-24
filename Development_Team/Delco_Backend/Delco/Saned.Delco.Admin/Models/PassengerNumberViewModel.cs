using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Saned.Delco.Data.Core;
using Saned.Delco.Data.Persistence;

namespace Saned.Delco.Admin.Models
{
    public class PassengerNumberViewModel
    {
        [ScaffoldColumn(false)]
        public long Id { get; set; }

        [Display(Name = "عدد الركاب")]
        [Range(1, 1000,ErrorMessage = "{2} عدد الركاب بين {1} و")]
        [ExistingPassengerNumber(ErrorMessage = "هذا العدد موجود بالفعل")]

        [Required(ErrorMessage = "يجب ادخال عدد الركاب")]
        public long Value { get; set; }

        [Display(Name= "الاسم")]
        [Required(ErrorMessage = "يجب ادخال الاسم")]
        public string Name { get; set; }
    }

    public class ExistingPassengerNumberAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IUnitOfWorkAsync unitOfWork = new UnitOfWorkAsync();
            PassengerNumberViewModel number = (PassengerNumberViewModel)validationContext.ObjectInstance;
            bool exist = false;
            if (number.Id!=0)
                exist = unitOfWork.PassengerNumberRepository.All().Any(u => u.Value == (long)value && u.Id!= number.Id);
            else           
              exist = unitOfWork.PassengerNumberRepository.All().Any(u => u.Value == (long)value );



            if (exist)
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;

        }
    }
}
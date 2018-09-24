using System.ComponentModel.DataAnnotations;
using System.Linq;
using Saned.Delco.Admin.Models;
using Saned.Delco.Data.Core;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Persistence;

namespace Saned.Delco.Admin.CustomValidators
{
    public class ExistingPathAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IUnitOfWorkAsync unitOfWork = new UnitOfWorkAsync();
            ExternalPathViewModel path = (ExternalPathViewModel)validationContext.ObjectInstance;

            
            //var exist =
            //    unitOfWork.PathRepository.All().Any(u => path.Id==0 &&
            //                                             u.FromCityId == path.FromCityId &&
            //                                             u.ToCityId == path.ToCityId && u.Type==PathTypesEnum.External);


            bool exist = false;
            if (path.Id == 0)
            {
                exist =
               unitOfWork.PathRepository.All().Any(u => path.Id == 0 &&
                                                        u.FromCityId == path.FromCityId &&
                                                         u.ToCityId == path.ToCityId &&  u.Type == PathTypesEnum.External);
            }
            else
            {
                exist = unitOfWork.PathRepository.All().Any(u => u.Id != path.Id &&
                                                       u.FromCityId == path.FromCityId &&
                                                        u.ToCityId == path.ToCityId &&  u.Type == PathTypesEnum.External);
            }
            if (exist)
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;
           
        }
    }
}
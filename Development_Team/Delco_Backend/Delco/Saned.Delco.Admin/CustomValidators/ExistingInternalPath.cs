using System.ComponentModel.DataAnnotations;
using System.Linq;
using Saned.Delco.Admin.Models;
using Saned.Delco.Data.Core;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Persistence;

namespace Saned.Delco.Admin.CustomValidators
{
    public class ExistingInternalPath : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IUnitOfWorkAsync unitOfWork = new UnitOfWorkAsync();
            InternalPathViewModel path = (InternalPathViewModel)validationContext.ObjectInstance;
            bool exist = false;
            if (path.Id == 0)
            {
                 exist =
                unitOfWork.PathRepository.All().Any(u => path.Id == 0 &&
                                                         u.FromCityId == path.FromCityId &&
                                                         u.Type == PathTypesEnum.Internal);
            }
            else
            {
                 exist = unitOfWork.PathRepository.All().Any(u =>  u.Id!=path.Id &&
                                                        u.FromCityId == path.FromCityId &&
                                                        u.Type == PathTypesEnum.Internal);
            }
                    
                
            if (exist)
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;

        }
    }
}
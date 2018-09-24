using System.ComponentModel.DataAnnotations;
using Saned.Delco.Admin.Models;

namespace Saned.Delco.Admin.CustomValidators
{
    public class CampareWithToCityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ExternalPathViewModel path = (ExternalPathViewModel)validationContext.ObjectInstance;                      
            if (path.FromCityId== path.ToCityId)
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;

        }
    }
}
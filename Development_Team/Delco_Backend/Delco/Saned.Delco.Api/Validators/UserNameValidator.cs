using System.ComponentModel.DataAnnotations;

namespace Saned.Delco.Api.Validators
{
    sealed public class UserNameValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = true;
            // Add validation logic here.
            return result;
        }
    }
}
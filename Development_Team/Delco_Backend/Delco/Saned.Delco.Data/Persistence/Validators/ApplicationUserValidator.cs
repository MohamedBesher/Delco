using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Infrastructure;
using Saned.Delco.Data.Core;

namespace Saned.Delco.Data.Persistence.Validators
{
    public class ApplicationUserValidator<TUser> : UserValidator<TUser>
        where TUser : ApplicationUser
    {
        public bool PhoneIsRequire { get; set; }
        private ApplicationUserManager<TUser> Manager { get; }

        private readonly IUnitOfWorkAsync _unitOfWork;
        private ApplicationDbContext _context;

        public ApplicationUserValidator(ApplicationUserManager<TUser> manager) : base(manager)
        {
            Manager = manager;
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWorkAsync(_context);
        }

        public override async Task<IdentityResult> ValidateAsync(TUser item)
        {
            IdentityResult baseResult = await base.ValidateAsync(item);
            List<string> errors = new List<string>(baseResult.Errors);

            if (Manager != null)
            {
                var otherAccount = await Manager.FindByPhoneNumberUserManagerAsync(item.PhoneNumber);
                if (otherAccount != null && otherAccount.Id != item.Id)
                {
                    string errorMsg = "Phone Number '" + item.PhoneNumber + "' is already taken.";
                    errors.Add(errorMsg);
                }

                var otherAccount2 = await Manager.FindByEmailUserManagerAsync(item.Email);
                if (otherAccount2 != null && otherAccount2.Id != item.Id)
                {
                    string errorMsg = "Email '" + item.Email + "' is already taken.";
                    errors.Add(errorMsg);
                }

                

            }




            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
        }

    }
}
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Services;
using Saned.Delco.Data.Persistence.Validators;

namespace Saned.Delco.Data.Persistence.Infrastructure
{
    public class ApplicationUserManager<TUser> : UserManager<TUser> where TUser : ApplicationUser
    {

        public ApplicationUserManager() : base(new ApplicationUserStore<TUser>())
        {


        }

        public ApplicationUserManager(IUserCustomStore<TUser> store)
            : base(store)
        {
            UserValidator = new ApplicationUserValidator<TUser>(this)
            {
                //RequireUniqueEmail = true,
            };

            this.EmailService = new EmailService();
            //var provider = new DpapiDataProtectionProvider("AppName");
            //this.UserTokenProvider = new DataProtectorTokenProvider<TUser, string>(provider.Create("ASP.NET Identity")); ; //new DataProtectorTokenProvider<ApplicationUser, long>(provider.Create("PasswordReset"));

        }

        //public virtual Task<ApplicationUser> FindByPhoneNumberUserManagerAsync(string phoneNumber)
        //{
        //    IUserCustomStore<ApplicationUser> userCustomStore = Store as IUserCustomStore<ApplicationUser>;
        //    if (phoneNumber == null)
        //    {
        //        throw new ArgumentNullException();
        //    }
        //    return userCustomStore?.FindByPhoneNumberAsync(phoneNumber);
        //}
        public virtual Task<ApplicationUser> FindByPhoneNumberUserManagerAsync(string phoneNumber)
        {
            IUserCustomStore<ApplicationUser> userCustomStore = Store as IUserCustomStore<ApplicationUser>;
            //if (phoneNumber == null)
            //{
            //    return new Task<ApplicationUser>();
            //    //throw new ArgumentNullException();
            //}
            return userCustomStore?.FindByPhoneNumberAsync(phoneNumber);

        }

        public virtual Task<ApplicationUser> FindByEmailUserManagerAsync(string phoneNumber)
        {
            IUserCustomStore<ApplicationUser> userCustomStore = Store as IUserCustomStore<ApplicationUser>;
            //if (phoneNumber == null)
            //{
            //    return new Task<ApplicationUser>();
            //    //throw new ArgumentNullException();
            //}
            return userCustomStore?.FindByEmailAddressAsync(phoneNumber);


        }
    }
}


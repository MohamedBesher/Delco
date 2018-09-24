using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.Infrastructure
{
    public class ApplicationUserStore<TUser> : 
        UserStore<TUser>, 
        IDisposable, 
        IUserCustomStore<TUser> where TUser : ApplicationUser
    {
        public ApplicationUserStore() : base(new ApplicationDbContext())
        {

        }
        public Task<TUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            return Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.PhoneNumber != null);
        }
        public Task<TUser> FindByEmailAddressAsync(string email)
        {
            return Users.FirstOrDefaultAsync(x => x.Email == email && x.Email != null);
        }
        protected override void Dispose(bool isDisposing)
        {
            if (!isDisposing)
            {
                return;
            }


        }
    }
}
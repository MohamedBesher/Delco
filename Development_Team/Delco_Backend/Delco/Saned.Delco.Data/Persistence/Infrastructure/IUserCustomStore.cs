using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Data.Persistence.Infrastructure
{
    public interface IUserCustomStore<TUser> : IUserStore<TUser> , IDisposable where TUser : ApplicationUser, IUser<string>
    {
        Task<TUser> FindByPhoneNumberAsync(string phoneNumber);
        Task<TUser> FindByEmailAddressAsync(string phoneNumber);
    }
}
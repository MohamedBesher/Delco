using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Infrastructure;

namespace Saned.Delco.Data.Persistence.Providers
{
    public class ApplicationTokenProvider<TUser> : IUserTokenProvider<ApplicationUser, string> where TUser : class, IUser
    {
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(3, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(1, true));
            return builder.ToString();
        }
        private string GenerateId(Guid id)
        {
            long i = id.ToByteArray().Aggregate<byte, long>(1, (current, b) => current * ((int)b + 1));
            return String.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        public async Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            string resetToken = GetPassword();//GenerateId(Guid.NewGuid());
            if (purpose == "Confirmation")
            {
                user.ConfirmedToken = resetToken;
                await manager.UpdateAsync(user);
            }

            if (purpose == "ResetPassword")
            {
                user.ResetPasswordlToken = resetToken;
                await manager.UpdateAsync(user);
            }
            return await Task.FromResult<string>(resetToken.ToString());
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            //if (manager == null) throw new ArgumentNullException();
            //else
            //{
            //    return Task.FromResult<bool>(manager.SupportsUserEmail);
            //}
            return Task.FromResult(true);
        }

        public Task NotifyAsync(string token, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return Task.FromResult<int>(0);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            if (purpose == "Confirmation")
            {
                return Task.FromResult<bool>(user.ConfirmedToken.ToString() == token);
            }
            else if (purpose == "ResetPassword")
            {
                return Task.FromResult<bool>(user.ResetPasswordlToken.ToString() == token);
            }
            else
            {
                return Task.FromResult<bool>(false);
            }
        }
    }
}
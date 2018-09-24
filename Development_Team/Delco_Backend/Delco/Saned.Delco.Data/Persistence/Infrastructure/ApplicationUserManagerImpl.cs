using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Providers;

namespace Saned.Delco.Data.Persistence.Infrastructure
{
    public class ApplicationUserManagerImpl : ApplicationUserManager<ApplicationUser>
    {
        public ApplicationUserManagerImpl() : base(new ApplicationUserStoreImpl())
        {
            this.UserTokenProvider = new ApplicationTokenProvider<ApplicationUser>();

        }

        
    }
}
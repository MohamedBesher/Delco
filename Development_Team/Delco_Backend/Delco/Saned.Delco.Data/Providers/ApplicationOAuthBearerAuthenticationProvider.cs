using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Data.Providers
{
    public class ApplicationOAuthBearerAuthenticationProvider : OAuthBearerAuthenticationProvider
    {
        private readonly AuthRepository _repo = null;
        public ApplicationOAuthBearerAuthenticationProvider()
        {
            _repo = new AuthRepository();
        }
        public override async Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            var result = base.ValidateIdentity(context);


            if (context.IsValidated)
            {
                var ticket = context.Ticket;
                if (ticket != null && ticket.Identity.IsAuthenticated && ticket.Properties.ExpiresUtc > DateTime.UtcNow)
                {
                    string name = ticket.Identity.Name;
                    if (!string.IsNullOrEmpty(name))
                    {
                        ApplicationUser u = await _repo.FindUserByUserName(name);
                        if (u == null)//TODO: put your server side condition here
                        {
                            context.SetError("User Not found!");
                        }
                        else if(u.IsSuspend.HasValue && u.IsSuspend.Value)
                            context.SetError("User Has Suspended!");
                    }

                }
            }



            //return result;

        }

    }
}

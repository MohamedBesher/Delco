using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Infrastructure;
using Saned.Delco.Data.Persistence.Repositories;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Core;

namespace Saned.Delco.Data.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUnitOfWorkAsync _unitOfWork;
        private ApplicationDbContext _context;

        public SimpleAuthorizationServerProvider()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWorkAsync(_context);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {




            //comment to Ahmed Maher
            string deviceId = context.Parameters.Where(f => f.Key == "DeviceId").Select(f => f.Value).SingleOrDefault()[0];
            context.OwinContext.Set<string>("DeviceId", deviceId);




            string clientId;
            string clientSecret;
            Client client;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            using (AuthRepository repo = new AuthRepository())
            {
                client = repo.FindClient(context.ClientId);
            }

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string role = context.OwinContext.Get<string>("role");
            string deviceId = context.OwinContext.Get<string>("DeviceId");




            if (string.IsNullOrWhiteSpace(role))
                role = "User";




            ApplicationUser user;
            string roles = "";
            //using (AuthRepository repo = new AuthRepository())
            //{

            //}
            AuthRepository repo = new AuthRepository();

            user = await repo.FindUser(context.UserName, context.Password);


            ////  context.
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            else
            {

                // comment for ahmed Maher
                if (repo.IsInRole(user.Id, RolesEnum.Agent.ToString()))
                {
                    var deviceSetting = _unitOfWork.DeviceSettingRepository.Find(x => x.ApplicationUserId == user.Id);
                    if (deviceSetting != null)
                    {
                        if (deviceSetting.DeviceId != deviceId)
                        {
                            context.SetError("invalid_grant", "You must login from your device");
                            return;
                        }
                    }
                }


                //!repo.IsInRole(user.Id, role)
                if (!repo.IsInRole(user.Id, RolesEnum.User.ToString()) && !repo.IsInRole(user.Id, RolesEnum.Agent.ToString()))
                {
                    context.SetError("invalid_grant", "You Have No Right To Enter");
                    return;
                }


                if (CheckUserValid(context, user)) return;

            }

            var roleslist = await repo.GetUserRoles(user.Id);
            roles = string.Join(",", (roleslist));





            var identity = SetClaimsIdentity(context, user);
            await repo.UpdateStatus(user, true);

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", context.ClientId ?? string.Empty
                    },
                    {
                        "userName", context.UserName
                    },
                    {
                      "roles",roles
                    },
                    {
                        "favoriteOrderType",user.Type.ToString()
                    }
                ,
                 {
                        "isNewNotifications",user.IsNotified.ToString()
                    },


                });



            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);


        }

        private static bool CheckUserValid(OAuthGrantResourceOwnerCredentialsContext context, ApplicationUser user)
        {
            bool isDelete = false;
            bool isSuspend = false;

            if (user.IsDeleted != null) isDelete = user.IsDeleted.Value;
            if (user.IsSuspend != null) isSuspend = user.IsSuspend.Value;
            var isMobileConfirmed = user.PhoneNumberConfirmed;
            if (!isMobileConfirmed || isDelete)
            {
                if (isDelete)
                    context.SetError("invalid_grant", "User are Arhieve");
                if (!isMobileConfirmed)
                    context.SetError("invalid_grant", "Phone Need To Confirm");
                if (isSuspend)
                    context.SetError("invalid_grant", "SuspendedUser");

                return true;



            }
            return false;
        }

        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, ApplicationUser user)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("user_id", user.Id));
            using (AuthRepository repo = new AuthRepository())
            {
                var userRoles = repo.GetRoles(user.Id);
                foreach (var role in userRoles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
            return identity;
        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}
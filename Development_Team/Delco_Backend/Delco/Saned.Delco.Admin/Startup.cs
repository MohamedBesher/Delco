using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartupAttribute(typeof(Saned.Delco.Admin.Startup))]
namespace Saned.Delco.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                ExpireTimeSpan = TimeSpan.FromHours(1),
            });
            ConfigureAuth(app);
        }
    }
}

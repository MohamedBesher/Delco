using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Saned.Delco.Data.Providers;

[assembly: OwinStartup(typeof(Saned.Delco.Api.Startup))]

namespace Saned.Delco.Api
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"> this parameter will be supplied by the host at run-time. 
        /// This “app” parameter is an interface which will be used to compose the application for our Owin server.</param>
        public void Configuration(IAppBuilder app)
        {

            ConfigureOAuth(app);
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR(new HubConfiguration()
                {
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true,
                    // for old browser
                    EnableJSONP = true
                });
            });


           
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        public void ConfigureOAuth(IAppBuilder app)
        {


            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            //{
            //   // Provider = new ApplicationOAuthBearerAuthenticationProvider(),

            //}



            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(43200),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
            GoogleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "204314378747-seuub0efs5tsr3bpu88cdkk7h6o88tk0.apps.googleusercontent.com",
                ClientSecret = "kE4Kyn_SBt7F1ZA7rQeAE1rd",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(GoogleAuthOptions);

            //Configure Facebook External Login
            FacebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "276971536031476",
                AppSecret = "359b0507ba17d5f5dca805b0e57b8a5a",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(FacebookAuthOptions);

        }
    }
}

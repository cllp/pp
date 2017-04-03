using PP.Core.Context;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using Microsoft.Owin;
using PP.Api.Providers;
using PP.Core;
using PP.Core.Helpers;
using Microsoft.Owin.Cors;


namespace PP.Api
{
    public partial class Startup
    {
        static Startup()
        {
            OAuthBearerOptions = new OAuthAuthorizationServerOptions
            {
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                Provider = new SimpleAuthorizationServerProvider()
                //Provider = new ApplicationOAuthProvider()
            };

            var obo = new OAuthAuthorizationServerOptions();
        }

        public static OAuthAuthorizationServerOptions OAuthBearerOptions { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            //Enable Cors Policy
            //Microsoft.Owin.Cors.CorsOptions corsOptions = new Microsoft.Owin.Cors.CorsOptions();
            //corsOptions.PolicyProvider = new ConfigCorsPolicy(Constants.APICrossDomainOrigins);
            //app.UseCors(corsOptions);

            app.UseCors(CorsOptions.AllowAll);

            //initialize the identitycontext
            //IdentityContext.Initialize(new FormsIdentity<User>());
            IdentityContext.Initialize(new Identity());

            //Microsoft.Owin.Cors.CorsOptions corsOptions = new Microsoft.Owin.Cors.CorsOptions();
            //corsOptions.PolicyProvider = new ConfigCorsPolicy(corsOrigin);
            //app.UseCors(corsOptions);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthBearerOptions);

        }
    }
}
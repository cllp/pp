using Microsoft.Owin.Security.OAuth;
using PP.Core.Helpers;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PP.Api
{
    public static class WebApiConfig
    {
        //private static string crossDomainOrigins = ConfigurationManager.AppSettings["APICrossDomainOrigins"];

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.EnableCors();
            //Enable cross domain calls to API (set origin in web.config)
            //if (!string.IsNullOrEmpty(crossDomainOrigins))
            //{
               
            //}

            // var cors = new EnableCorsAttribute(Constants.APICrossDomainOrigins, "*", "*");
            //config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}

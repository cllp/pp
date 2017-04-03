
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using PP.Core;
using PP.Core.Interfaces;
using PP.Core.Model;
using RestSharp;
using System;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PP.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public ILog log = CoreFactory.Log;

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // OAuth2 supports the notion of client authentication
            // this is not used here
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (context.UserName.ToLower().Equals("null") || context.Password.ToLower().Equals("null"))
                throw new AuthenticationException("Username or Password is not provided");

            UserView user = null;

            if (context.Password.ToLower().Equals("yammer", StringComparison.CurrentCultureIgnoreCase))
            {
                //log.Info("Authenticating with yammer (token,yammerid): " + context.UserName, false);
                user = CoreFactory.AppRepository.Validate(GetYammerUser(context.UserName), context.Password);
            }
            else
            {
                //log.Info("Authenticating with forms (username): " + context.UserName, false);
                user = CoreFactory.AppRepository.Validate(context.UserName, context.Password);
            }

            //user.Password = "hidden";

            if (user != null)
            {
                var id = new ClaimsIdentity(context.Options.AuthenticationType);
                id.AddClaim(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user)));
                id.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                context.Validated(id);
            }
            else
            {
                var msg = string.Format("User '{0}' could not be validated", context.UserName);
                log.Error(msg, new AuthenticationException(msg), false);
                context.Rejected();
            }
        }

        private string GetYammerUser(string token)
        {
            string[] yammerdata = token.Split(',');
            return YammerUser(yammerdata[0], yammerdata[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="yammerId"></param>
        /// <returns></returns>
        public string YammerUser(string token, string yammerId)
        {
            try
            {
                var url = @"https://www.yammer.com/api/v1/users/" + yammerId + ".json";

                string content = GetResponseContent(url, token);

                var user = (dynamic)JsonConvert.DeserializeObject(content);
                var email = user.contact.email_addresses[0].address.Value;

                log.Info("Retriving user from yammer (token,yammerid): " + token + "," + yammerId + ". User: " + content, false);

                //u.FullName = user.full_name.Value;
                //u.Id = user.id.Value;
                //u.MugshotUrl = user.mugshot_url.Value;
                //u.NetworkId = user.network_id.Value;
                //u.NetworkName = user.network_name.Value;
                //u.Url = user.url.Value;
                //u.WebUrl = user.web_url.Value;
                //u.Email = user.contact.email_addresses[0].address.Value;

                return email;
            }
            catch (Exception ex)
            {
                log.Error("Retriving user from yammer failed. (token,yammerid): " + token + "," + yammerId, ex, false);
                throw ex;
            }
        }

        private string GetResponseContent(string url, string token)
        {
            var client = new RestClient
            {
                BaseUrl = url
            };

            try
            {
                var request = new RestRequest();
                request.Method = Method.GET;
                if (!string.IsNullOrEmpty(token))
                {
                    request.AddHeader("Authorization", "Bearer " + token);
                }

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.Content;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Yammer Exception: " + response.StatusCode.ToString());

                    if (!string.IsNullOrEmpty(response.ErrorMessage))
                        sb.Append(". " + response.ErrorMessage);

                    if (!string.IsNullOrEmpty(response.Content))
                        sb.Append(". " + response.Content);

                    throw new ApplicationException(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }
}
using PP.Api.Attributes;
using PP.Api.Helpers;
using PP.Core.Context;
using PP.Core.Model;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PP.Api.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : BaseController
    {
        public AccountController()
        {
            // Supress redirection for web services
            HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = true;
        }

        [HttpGet]
        [Route("profile")]
        //[ApiAuthorize]
        [Authorize]
        public HttpResponseMessage Profile()
        {
            try
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<object>(new
                    {
                        IsAuthenticated = IdentityContext.Current.IsAuthenticated,
                        User = IdentityContext.Current.JsonUser,
                    }, Configuration.Formatters.JsonFormatter)
                };
            }
            catch (Exception ex)
            {
                log.Error("Error while getting profile. " + ex);
                throw ex;
            }
        }

        [HttpPost]
        [Route("changepassword")]
        [AllowAnonymous]
        public string ChangePassword(PasswordChange obj)
        {

            int result = appRepository.ChangePassword(obj);

            return result.ToString();

        }

        [HttpPost]
        [Route("register")]
        //[ApiAuthorize]
        [AllowAnonymous]
        public HttpResponseMessage Register(NewUser user)
        {
            try
            {
                var password = string.Empty;
                var status = RegisterUserStatus.Undefined;
                appRepository.CreateUser(user.Email, user.Name, out password, out status);

                if (status == RegisterUserStatus.Ok)
                {
                    ITextReplace replace = new TextReplace();
                    //replace.Add("%name%", pmv.UserName);
                    replace.Add("%username%", user.Email.Trim());
                    replace.Add("%password%", password);
                    string body = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "RegisterUser.html"));
                    // TODO replace with setting
                    MailService smtp = new MailService("http://communicationservice.woxion.com/MailService.asmx");
                    //bool result = smtp.Send("Projektplaneraren", "info@projektplaneraren.se", user.Email.Trim(), null, "Projektplaneraren", body);
                    bool result = smtp.Send("Projektplaneraren", "info@projektplaneraren.se", "claes-philip.staiger@stretch.se", null, "Projektplaneraren", body);
                }

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<object>(new
                    {
                        IsRegistered = status == RegisterUserStatus.Ok,
                        Status = status.ToString()
                        //Password = password
                        //IsAuthenticated = IdentityContext.Current.IsAuthenticated,
                        //User = IdentityContext.Current.JsonUser,
                    }, Configuration.Formatters.JsonFormatter)
                };
            }
            catch (Exception ex)
            {
                log.Error("Error while register new user. " + ex);
                throw ex;
            }
        }
    }
}
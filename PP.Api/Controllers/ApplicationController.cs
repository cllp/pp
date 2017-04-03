using PP.Api.Attributes;
using PP.Api.Controllers;
using PP.Api.Mapper;
using PP.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using PP.Core.Model;
using PP.Api.Helpers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;

namespace PP.Api.Controllers
{
    [RoutePrefix("application")]
    [ApiAuthorize]
    public class ApplicationController : BaseController
    {
        [HttpGet]
        [Route("version")]
        [AllowAnonymous]
        public string Version()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        [HttpGet]
        [Route("users")]
        [AllowAnonymous]
        public IEnumerable<UserView> GetUsers()
        {
            return appRepository.GetUsers();
        }

        [HttpGet]
        [Route("requestpasswordchange")]
        [AllowAnonymous]
        public string RequestPasswordChange([FromUri] PasswordChange obj)
        {           
            //1. create a password change request
            var user = appRepository.ChangePasswordRequest(obj.Email);

            //2. send a request emailmessage
            ITextReplace replace = new TextReplace();

            if(!string.IsNullOrEmpty(user.Name))
                replace.Add("%name%", user.Name);
            else
                replace.Add("%name%", "");

            replace.Add("%epost%", user.Email.Trim());
            if (user.ChangePasswordRequest != null)
                replace.Add("%url%", string.Format(obj.Url + "changepassword.html?key={0}", user.ChangePasswordRequest.Value));
            string body = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ChangePassword.html"));
            
            MailService smtp = new MailService("http://communicationservice.woxion.com/MailService.asmx");
            bool result = smtp.Send("Projektplaneraren", "info@projektplaneraren.se", user.Email.Trim(), null, "Ändra lösenord", body);
            return "Request Created";
        }

        [HttpPost]
        [Route("changepassword")]
        [AllowAnonymous]
        public string ChangePassword(PasswordChange obj)
        {
            int result = appRepository.ChangePassword(obj); 
            return result.ToString();
        }

        [HttpGet]
        [Route("mail")]
        [AllowAnonymous]
        public string Mail()
        {
            string email = "claes-philip.staiger@stretch.se";

            //2. send a request emailmessage
            ITextReplace replace = new TextReplace();
            replace.Add("%name%", "Claes-Philip");
            replace.Add("%username%", email);
            replace.Add("%yammer%", true ? "Aktivt" : "Inaktivt");
            //replace.Add("%url%", string.Format("http://projectplanner.azurewebsites.net/changepassword.html?key={0}", "mykey"));
            replace.Add("%url%", string.Format("http://projektplaneraren.se/changepassword.html?key={0}", "mykey"));
            string body = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ChangePassword.html"));

            MailService smtp = new MailService("http://communicationservice.woxion.com/MailService.asmx");
            bool result = smtp.Send("Projektplaneraren", "info@projektplaneraren.se", email, null, "Ändra lösenord", body);
            return "Request Created";
        }

        [HttpGet]
        [Route("setting/{name}")]
        [AllowAnonymous]
        public Settings GetSetting(string name)
        {
            var s = appRepository.GetSetting(name);
            return s;
        }

        [HttpGet]
        [Route("settings/{type}")]
        [AllowAnonymous]
        public IEnumerable<Settings> GetSettings(string type)
        {
            return appRepository.GetSettings(type);
        }

        [HttpGet]
        [Route("template/{key}")]
        [AllowAnonymous]
        public Template GetTemplate(string key)
        {
            return appRepository.GetTemplate(key);
        }

        [HttpGet]
        [Route("templates/{section}")]
        [AllowAnonymous]
        public IEnumerable<Template> GetTemplates(string section)
        {
            return appRepository.GetTemplates(section);
        }
    }
}
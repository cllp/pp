using PP.Core.Model;
using System;
using System.Collections.Generic;

namespace PP.Core.Interfaces
{
    public interface IAppRepository
    {
        UserView Validate(string email, string password);

        IEnumerable<UserView> GetUsers();

        UserView ChangePasswordRequest(string email);

        int ChangePassword(PasswordChange obj);

        Settings GetSetting(string name);

        IEnumerable<Settings> GetSettings(string type);

        /// <summary>
        /// creates a user and returns the password in clear text
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        //string CreateUser(string email);
        void CreateUser(string email, string name, out string password, out RegisterUserStatus status);

        /*
        [Key] nvarchar (50), --projectmember, confirmationemail etc
	    [Section] nvarchar (25), --email, pdf, helptext
	    [Format] nvarchar (25), --html, txt
         */

        IEnumerable<Template> GetTemplates(string section);

        IEnumerable<Template> GetTemplates(string section, string format);

        Template GetTemplate(string key);

        Template GetTemplate(int id);
    }
}

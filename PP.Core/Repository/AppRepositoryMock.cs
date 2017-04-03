using PP.Core.Interfaces;
using PP.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PP.Core.Repository
{
    public class AppRepositoryMock : IAppRepository
    {

        public IEnumerable<Project> SearchProjects(string search)
        {
            string json = String.Empty;

            using (StreamReader r = new StreamReader(@"C:\Source\PP\PP.Api\Mock\Projects.json"))
            {
                json = r.ReadToEnd();
            }

            IEnumerable<Project> array = (IEnumerable<Project>)JsonConvert.DeserializeObject(json, typeof(IEnumerable<Project>));

            return array.Where(s => s.Name.StartsWith(search));
        }

        public Project GetProjectByName(string name)
        {
            throw new NotImplementedException();
        }


        public Project GetProjectById(string name)
        {
            throw new NotImplementedException();
        }

        public UserView Validate(string email, string password)
        {
            throw new NotImplementedException();
        }


        public void UpdateProjectXml(int projectId, string xml)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<UserView> GetUsers()
        {
            throw new NotImplementedException();
        }


        public void WriteLog(string type, string message)
        {
            throw new NotImplementedException();
        }

        public void WriteLog(string type, string message, Exception ex)
        {
            throw new NotImplementedException();
        }


        public UserView ChangePasswordRequest(string email)
        {
            throw new NotImplementedException();
        }


        public int ChangePassword(PasswordChange obj)
        {
            throw new NotImplementedException();
        }


        public Settings GetSetting(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Settings> GetSettings(string type)
        {
            throw new NotImplementedException();
        }


        public string CreateUser(string email)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Template> GetTemplates(string section)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Template> GetTemplates(string section, string format)
        {
            throw new NotImplementedException();
        }

        public Template GetTemplate(string key)
        {
            throw new NotImplementedException();
        }

        public Template GetTemplate(int id)
        {
            throw new NotImplementedException();
        }


        public void CreateUser(string email, out string password, out RegisterUserStatus status)
        {
            throw new NotImplementedException();
        }


        public void CreateUser(string email, string name, out string password, out RegisterUserStatus status)
        {
            throw new NotImplementedException();
        }
    }
}

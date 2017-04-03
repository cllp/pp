using PP.Core.Helpers;
using PP.Core.Interfaces;
using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PP.Core.Helpers;
using PP.Core.Context;
using PP.Core.Exceptions;

namespace PP.Core.Repository
{
    public class AppRepository : RepositoryBase, IAppRepository
    {
        public UserView Validate(string email, string password)
        {
            string query = String.Empty;

            if (password.IsYammer())
                query = "ValidateYammerUser";
            else
                query = "ValidateFormsUser";

            try
            {
                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    var par = new Dapper.DynamicParameters();
                    par.Add("@Email", email);
                    par.Add("@Password", password);
                    var user = conn.Query<UserView>(query, par, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    log.Info("Validated user '" + user.DisplayName.Trim() + "' with email: '" + email + "'. Authentication method: '" + query + "'", false);

                    return user;
                }
            }
            catch (Exception ex)
            {
                log.Error("Validation of user: '" + email + "' failed", ex, false);
                throw;
            }
        }

        public UserView ChangePasswordRequest(string email)
        {
            try
            {
                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    //conn.Execute("UPDATE [User] SET ChangePasswordRequest = @ChangePasswordRequest WHERE Email = @Email", new { Email = email.Trim(), ChangePasswordRequest = System.Guid.NewGuid() });
                    conn.Execute("[ChangePasswordRequest]", new { Email = email }, commandType: CommandType.StoredProcedure);
                    return conn.Query<UserView>("SELECT * FROM [UserView] WHERE Email = @Email", new { Email = email.Trim() }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("ChangePasswordRequest with email: '" + email + "' failed", ex);
                throw;
            }
        }

        public void CreateUser(string email, string name, out string password, out RegisterUserStatus status)
        {
            password = string.Empty;

            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                //check if user already exists
                int id = conn.Query<int>("SELECT Id FROM [User] WHERE Email = @Email", new { Email = email.Trim() }).FirstOrDefault();

                //if user does not exists
                if (id > 0)
                {
                    status = RegisterUserStatus.AlreadyExists;
                    log.Warning("Cannot create user, email: '" + email.Trim() + "' already exists", false);
                    //throw new Exception("Cannot create user, a user with email: '" + email.Trim() + "' already exists");
                }
                else
                {

                    //check if the email is external
                    var isInternalQuery = @"
	                        SELECT TOP (1)
                            CASE WHEN o.Domain IS NULL THEN 0 ELSE 1 END as [OrganizationState]
                            FROM Organization o
                            WHERE o.Domain = SUBSTRING(@Email, CHARINDEX('@', @Email) + 1, len(@Email))";

                    int isInternal = conn.Query<int>(isInternalQuery, new { Email = email.Trim() }).FirstOrDefault();

                    if (isInternal > 0)
                    {
                        try
                        {
                            var par = new Dapper.DynamicParameters();
                            par.Add("@Name", name.Trim());
                            par.Add("@Email", email.Trim());

                            password = conn.Query<string>("[CreateUser]", par, commandType: CommandType.StoredProcedure).FirstOrDefault();
                            status = RegisterUserStatus.Ok;

                            log.Info("Created user: '" + name.Trim() + "' with email: '" + email.Trim() + "'", false);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Cannot create user with email: '" + email.Trim() + "' Message: " + ex.Message, false);
                            status = RegisterUserStatus.Error;
                        }
                    }
                    else
                    {
                        status = RegisterUserStatus.NoDomainMatch;
                        log.Warning("Cannot create user, the email domain does not match any domain in the database");
                    }
                }
            }
        }

        public int ChangePassword(PasswordChange obj)
        {
            try
            {
                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    var par = new Dapper.DynamicParameters();
                    par.Add("@Name", obj.Name);
                    par.Add("@Password", obj.Password);
                    par.Add("@ChangePasswordRequest", obj.Key);
                    int result = conn.Execute("ChangePassword", par, commandType: CommandType.StoredProcedure);

                    if (result == -1)
                    {
                        string msg = string.Format("ChangePassword with key: '{0}' failed. The request has expired!", obj.Key);
                        log.Warning(msg);
                        //throw new Exception(msg);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error("ChangePassword with key: '" + obj.Key + "' failed", ex);
                throw;
            }
        }

        public IEnumerable<UserView> GetUsers()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<UserView>("SELECT * FROM UserView");
            }
        }

        public Settings GetSetting(string name)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<Settings>("SELECT * FROM [Settings] WHERE Name = @Name", new { Name = name }).FirstOrDefault();
            }
        }

        public IEnumerable<Settings> GetSettings(string type)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<Settings>("SELECT * FROM [Settings] WHERE Type = @Type", new { Type = type });
            }
        }


        public IEnumerable<Template> GetTemplates(string section)
        {
            return GetTemplates(section, string.Empty);
        }

        public IEnumerable<Template> GetTemplates(string section, string format)
        {
            try
            {
                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    var par = new Dapper.DynamicParameters();
                    par.Add("@Section", section);

                    var query = @"SELECT * FROM Template WHERE Section = @Section";

                    if (!string.IsNullOrEmpty(format))
                    {
                        query = " AND Format = @Format";
                        par.Add("@Format", format);
                    }

                    return conn.Query<Template>(query, par);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while quering templates", ex);
                throw;
            }
        }

        public Template GetTemplate(string key)
        {
            try
            {
                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    return conn.Query<Template>("SELECT * FROM Template WHERE Key = @Key", new { Key = key }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error while quering template with key {0}", key), ex);
                throw;
            }
        }

        public Template GetTemplate(int id)
        {
            try
            {
                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    return conn.Query<Template>("SELECT * FROM Template WHERE Id = @Id", new { Id = id }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error while quering template with id {0}", id), ex);
                throw;
            }
        }
    }
}

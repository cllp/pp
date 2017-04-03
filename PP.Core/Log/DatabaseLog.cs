using PP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PP.Core.Model.Enum;
using PP.Core.Context;
using System.Data.SqlClient;
using PP.Core.Model;
using PP.Core.Helpers;
using Dapper;

namespace PP.Core.Log
{
    public class DatabaseLog : ILog
    {
        public void Info(string message)
        {
            WriteLog("Info", message, true);
        }

        public void Info(string message, bool identity)
        {
            WriteLog("Info", message, identity);
        }

        public void Warning(string message)
        {
            WriteLog("Warning", message, true);
        }

        public void Warning(string message, Exception ex)
        {
            WriteLog("Warning", message, ex, true);
        }

        public void Warning(string message, bool identity)
        {
            WriteLog("Warning", message, identity);
        }

        public void Warning(string message, Exception ex, bool identity)
        {
            WriteLog("Warning", message, ex, identity);
        }

        public void Error(string message)
        {
            WriteLog("Error", message, true);
        }

        public void Error(string message, Exception ex)
        {
            WriteLog("Error", message, ex, true);
        }

        public void Error(string message, bool identity)
        {
            WriteLog("Error", message, identity);
        }

        public void Error(string message, Exception ex, bool identity)
        {
            WriteLog("Error", message, ex, identity);
        }

        public void Debug(string message, LogLevel level, bool identity)
        {
            WriteLog("Debug", message, identity);
        }

        public void Debug(string message, LogLevel level)
        {
            throw new NotImplementedException();
        }

        private void WriteLog(string type, string message, bool identity)
        {
            int userId = 0;

            if (identity)
            {
                if (IdentityContext.IsInitialized)
                {
                    if (IdentityContext.Current != null)
                        if (IdentityContext.Current.User != null)
                            userId = IdentityContext.Current.User.Id;
                }
            }

            var par = new Dapper.DynamicParameters();
            par.Add("@UserId", userId);
            par.Add("@Type", type);
            par.Add("@Message", message);

            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                conn.Execute("INSERT INTO [Log] (UserId, [Type], [Message]) VALUES (@UserId, @Type, @Message)", par);
            }
        }

        private void WriteLog(string type, string message, Exception ex, bool identity)
        {
            WriteLog(type, message + ". ExceptionMessage: " + ex.Message + ". StackTrace: " + ex.StackTrace, identity);
        }



    }
}

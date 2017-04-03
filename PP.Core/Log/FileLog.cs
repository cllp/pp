using PP.Core.Context;
using PP.Core.Helpers;
using PP.Core.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using PP.Core.Model.Enum;

namespace PP.Core.Log
{
    public class FileLog : ILog
    {
        #region Public methods

        public void Info(string message)
        {
            WriteEntry(message, LogLevel.Info);
        }

        public void Warning(string message)
        {
            WriteEntry(message, LogLevel.Warn);
        }

        public void Warning(string message, Exception ex)
        {
            WriteEntry(message + Environment.NewLine +
                ex.Message +
                GetExtraInfo(ex), LogLevel.Warn);
        }

        public void Error(string message)
        {
            WriteEntry(message, LogLevel.Error);
        }

        public void Error(string message, Exception ex)
        {
            WriteEntry(message + Environment.NewLine +
                ex.Message + Environment.NewLine +
                GetExtraInfo(ex), LogLevel.Error);
        }

        #endregion

        #region Private methods

        private string GetExtraInfo(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            if (ex != null)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    sb.Append(ex.InnerException.Message);
                    sb.Append(Environment.NewLine);
                }

                if (ex.StackTrace != null)
                {
                    sb.Append(ex.StackTrace);
                    sb.Append(Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        private void WriteEntry(string message, LogLevel level)
        {
            try
            {
                if(IdentityContext.Current.IsAuthenticated)
                {
                    message = message + Environment.NewLine + "User: " + IdentityContext.Current.JsonUser;
                }

                string filename = GetPathAndFileName();
                System.IO.File.AppendAllText(filename, string.Format("{0} {1}: {2}{3}", DateTime.Now.ToString(), level.ToString(), message, Environment.NewLine));

                Debug(message, level);
            }
            catch
            {
                throw;
            }
        }

        private string GetPathAndFileName()
        {
            string dir = EnvironmentHelper.LogDir; //Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Logs\");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return Path.Combine(dir, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
        }

        #endregion

        public void Debug(string message, LogLevel level)
        {
            bool debug = false;
            bool.TryParse(ConfigurationManager.AppSettings["Debug"], out debug);

            if (debug)
                System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}: {2}{3}", DateTime.Now.ToString(), level.ToString(), message, Environment.NewLine), "PP");
        }


        public void Info(string message, bool identity)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message, bool identity)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message, Exception ex, bool identity)
        {
            throw new NotImplementedException();
        }

        public void Error(string message, bool identity)
        {
            throw new NotImplementedException();
        }

        public void Error(string message, Exception ex, bool identity)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message, LogLevel level, bool identity)
        {
            throw new NotImplementedException();
        }
    }
}

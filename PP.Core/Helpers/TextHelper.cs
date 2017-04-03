using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PP.Core.Helpers
{
    public static class TextHelper
    {
        public static string GetStringAsString(object o)
        {
            if (o != null)
            {
                return o.ToString();
            }
            return string.Empty;
        }

        public static int GetStringAsInteger(object o)
        {
            if (o != null)
            {
                return GetStringAsInteger(o.ToString());
            }
            return -1;
        }

        public static int GetStringAsInteger(string s)
        {
            try
            {
                return int.Parse(s);
            }
            catch
            {
                return -1;
            }
        }

        public static double GetStringAsDouble(object o)
        {
            if (o != null)
            {
                return GetStringAsDouble(o.ToString());
            }
            return 0.0;
        }

        public static double GetStringAsDouble(string s)
        {
            try
            {
                return double.Parse(s);
            }
            catch
            {
                return 0.0;
            }
        }

        public static bool IsYammer(this string password)
        {
            if (password != null && password.Trim().ToLower().Equals("yammer"))
                return true;
            else
                return false;

            //bool valid = false;
            //if (number.Length > 5 && number.Length < 16) //length between 6 and 15
            //    if (number.All(char.IsDigit))
            //        valid  = true;

            //return valid;
        }

    }
}

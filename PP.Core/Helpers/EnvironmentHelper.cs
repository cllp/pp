using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Helpers
{
    public static class EnvironmentHelper
    {
        public static string LogDir 
        {
            get
            {
                return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Logs\");
            }

        }

        public static string PdfDir
        {
            get
            {
                return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Pdf\");
            }

        }

        public static string ReportDir
        {
            get
            {
                return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"report\");
            }

        }

        public static string ImgDir
        {
            get
            {
                return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Templates\Images\");
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PP.Core.Helpers
{
    public static class JsonHelper
    {
        public static string GetJsonFromFile(string fileName)
        {
            string json = String.Empty;

            if (!fileName.EndsWith(".json"))
            {
                fileName = string.Concat(fileName, ".json");
            }

            //string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Mock/" + fileName);

            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(path, "Mock", fileName);

            using (System.IO.StreamReader r = new System.IO.StreamReader(filePath))
            {
                json = r.ReadToEnd();
            }

            return json;
        }

        public static T GetJsonFromFile<T>(string fileName)
        {
            string json = String.Empty;

            if (!fileName.EndsWith(".json"))
            {
                fileName = string.Concat(fileName, ".json");
            }

            //string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Mock/" + fileName);

            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(path, "Mock", fileName);

            using (System.IO.StreamReader r = new System.IO.StreamReader(filePath))
            {
                json = r.ReadToEnd();
            }

            return (T)JsonConvert.DeserializeObject(json, typeof(T));
        }
    }
}

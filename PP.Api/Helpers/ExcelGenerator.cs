using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PP.Core.Helpers;
using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using PP.Core.Logic;
using System.Reflection;
using System.Text;
using System.Data;

namespace PP.Api.Helpers
{
    public static class ExcelGenerator
    {
        public static string GenerateExcel(DataTable dt, string name)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmm") + "." + name + ".csv";
            var path = EnvironmentHelper.ReportDir + fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Create(path).Close();

            string delimter = ";";

            List<string[]> output = new List<string[]>();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            output.Add(columnNames.ToArray());

            using (System.IO.TextWriter writer = new StreamWriter(path, true, Encoding.UTF8))
            {
                foreach (DataRow row in dt.Rows)
                {
                    List<string> r = new List<string>();

                    //for all properties of the item
                    for (int i = 0; i < columnNames.Count(); i++)
                    {
                        var value = row[i].ToString().Replace(";", "").Replace(",", "");
                        r.Add(value);
                    }

                    output.Add(r.ToArray());
                }

                for (int index = 0; index < output.Count; index++)
                {
                    writer.WriteLine(string.Join(delimter, output[index]));
                }
            }

            return fileName;
        }

        public static string GenerateExcel(IEnumerable<object> items, int id)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmm") + "." + id.ToString() + ".csv";
            var path = EnvironmentHelper.ReportDir + fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Create(path).Close();
            string delimter = ";";

            PropertyInfo[] myProps = items.First().GetType().GetProperties();//typeof(T).GetProperties();
            List<string> headers = new List<string>();

            foreach (var prop in myProps)
            {
                if (IsType(prop.PropertyType))
                    headers.Add(prop.Name);
            }

            List<string[]> output = new List<string[]>();

            output.Add(headers.ToArray());

            //TextWriter ws = new StreamWriter(path, true, Encoding.UTF8);

            using (System.IO.TextWriter writer = new StreamWriter(path, true, Encoding.UTF8))//File.CreateText(path))
            {
                foreach (var item in items)
                {
                    List<string> row = new List<string>();

                    //get the type of the item
                    var type = item.GetType();

                    //for all properties of the item
                    foreach (var prop in type.GetProperties())
                    {
                        //get the property value of the item 
                        if (IsType(prop.PropertyType))
                        {
                            var value = prop.GetValue(item);
                            row.Add(value.ToString());
                        }
                    }

                    output.Add(row.ToArray());
                }


                for (int index = 0; index < output.Count; index++)
                {
                    writer.WriteLine(string.Join(delimter, output[index]));
                }
            }

            return fileName;
        }

        private static bool IsType(Type type)
        {
            if (type.IsValueType || type.FullName == "System.String")
                return true;
            else
                return false;
        }

    }
}
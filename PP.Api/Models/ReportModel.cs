using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class ReportModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Procedure { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class ProjectCommentViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int AreaId { get; set; }
        public string Area { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public System.DateTime Date { get; set; }
        public string Text { get; set; }
        public string Writer { get; set; }
        public bool MyComment { get; set; }
    }
}
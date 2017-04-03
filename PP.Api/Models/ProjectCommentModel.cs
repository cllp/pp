using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class ProjectCommentModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ProjectCommentAreaId { get; set; }
        public int ProjectCommentTypeId { get; set; }
        public System.DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
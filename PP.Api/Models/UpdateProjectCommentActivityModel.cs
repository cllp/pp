using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class UpdateProjectCommentActivityModel
    {
        public int ProjectId { get; set; }
        public int ProjectCommentAreaId { get; set; }
        public int ProjectCommentTypeId { get; set; }
    }
}
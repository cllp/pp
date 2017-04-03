using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class ProjectVersionModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Comment { get; set; }
        public ProjectModel Data { get; set; }
        public System.DateTime? PublishedDate { get; set; }
        public int? PublishedBy { get; set; }
        public string PublishedByName { get; set; }
        public string Phase { get; set; }
        public int PhaseId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        //public string AreaName { get; set; }
        //public string PhaseId { get; set; }
        public string Phase { get; set; }
        public string Municipality { get; set; }
        public string County { get; set; }
        public bool Favorite { get; set; }
        public bool Member { get; set; }
        public bool Active { get; set; }
    }
}
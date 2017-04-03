using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public class ProjectMember : ReportItemBase
    {
        public string ProjectLeader { get; set; }   
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}

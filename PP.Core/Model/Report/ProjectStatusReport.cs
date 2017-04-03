using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public class ProjectStatusReport
    {
        private List<ProjectStatus> projects = new List<ProjectStatus>();

        public List<ProjectStatus> Projects
        {
            get { return projects; }
            set { projects = value; }
        }
    }
}

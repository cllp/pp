using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public partial class ProjectOverviewReport
    {
        private List<ProjectOverview> projects = new List<ProjectOverview>();

        public List<ProjectOverview> Projects
        {
            get { return projects; }
            set { projects = value; }
        }
    }
}

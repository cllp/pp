using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public class ProjectReport
    {
        private List<ProjectReportItem> projects = new List<ProjectReportItem>();

        public List<ProjectReportItem> Projects
        {
            get { return projects; }
            set { projects = value; }
        }

    }
}

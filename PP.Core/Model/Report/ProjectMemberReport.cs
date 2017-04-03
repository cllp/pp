using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public class ProjectMemberReport
    {
        private List<ProjectMember> projects = new List<ProjectMember>();

        public List<ProjectMember> Projects
        {
            get { return projects; }
            set { projects = value; }
        }
    }
}

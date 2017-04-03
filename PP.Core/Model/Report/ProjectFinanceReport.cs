using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public class ProjectFinanceReport
    {
        private List<ProjectFinance> projects = new List<ProjectFinance>();

        public List<ProjectFinance> Projects
        {
            get { return projects; }
            set { projects = value; }
        }
    }
}

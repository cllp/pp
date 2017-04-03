using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public partial class ProjectStatus : ReportItemBase
    {
        public string ProjectLeader { get; set; }
        public string ProjectActivityName { get; set; }
        public string ProjectActivityStatus { get; set; }
        public string ProjectInternalTime { get; set; }
        public string ProjectExternalTime { get; set; }
        public string ProjectTotalCost { get; set; }
        public string ProjectComment { get; set; }
    }
}

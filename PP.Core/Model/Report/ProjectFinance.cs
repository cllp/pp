using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public partial class ProjectFinance : ReportItemBase
    {
        public string ProjectLeader { get; set; }
        
        public string ProjectEstimate { get; set; }
        public string ProjectFinancedOrganization { get; set; }
        public string ProjectFinancedOrganizationSum { get; set; }
     }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public class ProjectOverview : ReportItemBase
    {
        

        /// <summary>
        /// ansvarig kommun
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// uppskattad kostnad
        /// </summary>
        public string EstmatedCost { get; set; }

        /// <summary>
        /// finansierat
        /// </summary>
        public string Financed { get; set; }

        /// <summary>
        /// projektägare
        /// </summary>
        public string ProjectOwner { get; set; }

        /// <summary>
        /// projektledare
        /// </summary>
        public string ProjectLeader { get; set; }

        /// <summary>
        /// budget summerad inten tid
        /// </summary>
        /// 
        public string BudgetSummaryInternalTime { get; set; }

        /// <summary>
        /// budget summerad edtern tid
        /// </summary>
        public string BudgetSummaryExternalTime { get; set; }

        /// <summary>
        /// budget summerad övriga kostnader
        /// </summary>
        public string BudgetSummaryOtherExpences { get; set; }

        /// <summary>
        /// budget summerad totalt
        /// </summary>
        public string BudgetSummaryTotal { get; set; }

        /// <summary>
        /// senaste uppföljning datum
        /// </summary>
        public string LatestFollowUpDate { get; set; }

        /// <summary>
        /// senaste uppföljning intern tid
        /// </summary>
        public string LatestFollowUpInternalTime { get; set; }

        /// <summary>
        /// senaste uppföljning extern tid
        /// </summary>
        public string LatestFollowUpExternalTime { get; set; }

        /// <summary>
        /// senaste uppföljning övriga kostnader
        /// </summary>
        public string LatestFollowUpOtherExpences { get; set; }

        /// <summary>
        /// senaste uppföljning totalt
        /// </summary>
        public string LatestFollowUpTotal { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public class ReportItemBase : IReport
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// projektnamn
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// projektområde
        /// </summary>
        public string ProjectArea { get; set; }

        /// <summary>
        /// projektfas
        /// </summary>
        public string ProjectPhase { get; set; }
    }
}

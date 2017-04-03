using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Enum
{
    public enum RiskAnalysisLevel
    {
        /// <summary>
        /// Låg
        /// </summary>
        /// 
        [Description("Låg")]
        Low = 1,
        /// <summary>
        /// Medel
        /// </summary>
        [Description("Medel")]
        Medium = 2,
        /// <summary>
        /// Hög
        /// </summary>
        [Description("Hög")]
        High = 3,



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Report
{
    public interface IReport
    {
        int Id { get; set; }

        string ProjectName { get; set; }

        string ProjectArea { get; set; }

        string ProjectPhase { get; set; }
    }
}

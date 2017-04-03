using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Enum
{
    public enum ProgramType
    {
        [Description("")]
        None = 0,
        [Description("Nationell")]
        National = 1,
        [Description("Län")]
        County = 2,
        [Description("Kommun")]
        Municipality = 3,
        [Description("Projekt")]
        Project = 4,
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model.Enum
{
    public enum Role
    {
        [Description("Användare")]
        Default = 0,
        [Description("Kommunadministratör")]
        OrganizationAdmin = 1,
        [Description("Länsadministratör")]
        CountyAdmin = 2,
        [Description("Systemadministratör")]
        SuperUser = 3
    }
}

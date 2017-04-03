using PP.Core.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model
{
    public class ProjectPermission
    {
        public PermissionSection Section { get; set; }
        public Permission Permission { get; set; }
    }
}

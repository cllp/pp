using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model
{
    public class UpdateProjectRoleResult : ProjectRole
    {
        private string status = string.Empty;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        
    }
}

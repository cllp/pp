using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model
{
    public class ProjectPermissions : List<ProjectPermission>
    {
        private List<int> editableRoleIds = new List<int>();

        public List<int> EditableRoleIds
        {
            get { return editableRoleIds; }
            set { editableRoleIds = value; }
        }
    }
}

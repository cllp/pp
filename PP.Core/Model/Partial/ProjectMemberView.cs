using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model
{
    public partial class ProjectMemberView : ModelBase
    {
        private List<ProjectRoleView> _projectRoles = new List<ProjectRoleView>();
        public bool IsYammerMember { get; set; }
        public User Member { get; set; }
        
        public List<ProjectRoleView> ProjectRoles
        {
            get { return _projectRoles; }
            set { _projectRoles = value; }
        }     
    }
}

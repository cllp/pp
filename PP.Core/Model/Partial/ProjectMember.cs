using System.Collections.Generic;

namespace PP.Core.Model
{
    public partial class ProjectMember : ModelBase
    {           
        public List<int> ProjectRoleIds { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}

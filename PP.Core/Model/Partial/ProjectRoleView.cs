using PP.Core.Model.Enum;
using System.ComponentModel.DataAnnotations;


namespace PP.Core.Model
{

    public partial class ProjectRoleView
    {
        public int MemberId { get; set; }

        public Permission Permission
        {
            get
            {
                return (Permission)this.PermissionId;
            }
        }
    }
}

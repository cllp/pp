using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class ProjectMemberRoleResultsModel
    {
        public string Status { get; set; }
        public RoleModel ProjectMemberRole { get; set; }
    }
}
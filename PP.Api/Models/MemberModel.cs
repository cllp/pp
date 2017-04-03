using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class MemberModel
    {
        private List<RoleModel> _memberRoles = new List<RoleModel>();
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<int> ProjectRoleIds { get; set; }
        public string Municipality { get; set; }
        public string Email { get; set; }
        public string Domain { get; set; }
        public string Organization { get; set; }

        public List<RoleModel> MemberRoles
        {
            get { return _memberRoles; }
            set { _memberRoles = value; }
        }
        public bool isSaved { get; set; }
        public bool memberExists { get; set; }
    }
}
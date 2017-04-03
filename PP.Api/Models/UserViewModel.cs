using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class UserViewModel
    {

        public int Id { get; set; }
        public string OrganizationState { get; set; }
        public string Organization { get; set; }
        public int? OrganizationId { get; set; }
        public string County { get; set; }
        public string Domain { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
    }
}
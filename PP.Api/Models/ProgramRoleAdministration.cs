using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class ProgramRoleAdministration
    {
        public int UserId { get; set; }
        public int ProgramTypeId { get; set; }
        public int ProjectRoleId { get; set; }
        public int? organizationId { get; set; }
    }
}
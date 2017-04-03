using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class CreateProjectModel
    {
        public int AreaId { get; set; }
        public int ProgramOwner { get; set; }
        public int ProjectCoordinator { get; set; }
        public int OrganizationId { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }        
    }
}
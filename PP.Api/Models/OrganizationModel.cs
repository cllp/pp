using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class OrganizationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string County { get; set; }
        public string Domain { get; set; }
    }
}
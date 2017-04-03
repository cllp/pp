using PP.Core.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class ProjectPermissionModel
    {       
            public string Section { get; set; }
            public Permission Permission { get; set; }
    }
}
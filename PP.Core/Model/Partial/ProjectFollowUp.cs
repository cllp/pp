using PP.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model
{
    public partial class ProjectFollowUp
    {
        [DapperIgnore]
        public decimal Balance { get; set; }
    }
}

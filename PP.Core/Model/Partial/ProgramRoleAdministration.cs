using PP.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model
{
    public partial class ProgramRoleAdministration
    {
        private bool saved = true;

        [DapperIgnore]
        public bool Saved
        {
            get { return saved; }
            set { saved = value; }
        }
        
    }
}

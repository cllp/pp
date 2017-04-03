using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PP.Core.Model
{
    [Serializable]
    public partial class SystemUser
    {
        public User User { get; set; }
    }
}

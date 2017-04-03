using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model
{
    public enum RegisterUserStatus
    {
        Undefined = 0,
        Ok = 1,
        AlreadyExists = 2,
        NoDomainMatch = 3,
        Error = 4

    }
}

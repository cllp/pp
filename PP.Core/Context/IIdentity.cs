using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Context
{
    public interface IIdentity
    {
        bool IsAuthenticated
        {
            get;
        }

        UserView User
        {
            get;
        }

        string JsonUser
        {
            get;
        }
    }
}

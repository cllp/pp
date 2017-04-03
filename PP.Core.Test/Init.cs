using PP.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace PP.Core.Test
{
    public static class Init
    {
        public static void User(string username, string password)
        {
            var user = CoreFactory.AppRepository.Validate(username, password);
            IdentityContext.Initialize(new TestIdentity(user));
        }
    }
}

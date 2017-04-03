using Newtonsoft.Json;
using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Tests
{
    public class TestIdentity : PP.Core.Context.IIdentity
    {
        private UserView user;

        public bool IsAuthenticated
        {
            get
            {
                return user != null;
            }
        }

        public UserView User
        {
            get { return user; }
            set { user = value; }
        }

        public string JsonUser
        {
            get
            {
                if (user != null)
                    return JsonConvert.SerializeObject(User);
                else
                    return string.Empty;
            }
        }

        public TestIdentity(UserView user)
        {
            this.user = user;
        }
    }
}

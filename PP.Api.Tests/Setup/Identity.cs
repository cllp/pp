using Newtonsoft.Json;
using PP.Core.Context;
using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.APi.Tests
{
    public class Identity : IIdentity
    {
        public bool IsAuthenticated
        {
            get { return true; }
        }

        public UserView User
        {
            get { return JsonConvert.DeserializeObject<dynamic>(JsonUser); }
        }

        public string JsonUser
        {
            get { return "{Id: 1, Name: 'Claes-Philip', Email: 'claes-philip@staiger.se'}"; }
        }
    }
}
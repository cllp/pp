using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Core.Model
{
    public class PasswordChange
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Guid Key { get; set; }
        public string Url { get; set; }
    }
}
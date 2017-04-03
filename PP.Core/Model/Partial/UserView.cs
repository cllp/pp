using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PP.Core.Helpers;
using PP.Core.Model.Enum;

namespace PP.Core.Model
{
    public partial class UserView
    {
        //public bool IsYammer
        //{
        //    get
        //    {
        //        return this.Password.IsYammer();
        //    }
        //}

        public Role Role
        {
            get
            {
                if (this.RoleId.HasValue)
                    return (Role)this.RoleId.Value;
                else
                    return Role.Default;
            }
        }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Name))
                    return this.Name;
                else
                    return this.Email;
            }
        }
    }
}

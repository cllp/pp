using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Interfaces
{
    public interface ISerializer
    {
        T Deserialize<T>(string data);

        string Serialize(object instance);
    }
}
